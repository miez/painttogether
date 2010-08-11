/* $HeadURL: http://ws201075/svn/deg/dotNet/trunk/Pegasus/src/HostApplicationForm.cs $
-----------------------------------------------------------------------------
        (c) by Martin Blankenstein

Dieses Dokument und die hierin enthaltenen Informationen unterliegen
dem Urheberrecht und duerfen ohne die schriftliche Genehmigung des
Herausgebers weder als ganzes noch in Teilen dupliziert oder reproduziert
noch manipuliert werden.

-----------------+------------------------------------------------------------
Version          : $Revision: 450 $
-----------------+------------------------------------------------------------
Last Change      : $LastChangedDate: 2009-02-23 18:26:54 +0100 (Mo, 23 Feb 2009) $
-----------------+------------------------------------------------------------
Last User        : $LastChangedBy: NLBERLIN\mblankenstein $
-----------------+------------------------------------------------------------
Beschreibung     :
-----------------+ 

-----------------+------------------------------------------------------------
Updates          :
-----------------+

$Id: HostApplicationForm.cs 450 2009-02-23 17:26:54Z NLBERLIN\mblankenstein $

*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Xml;
using log4net;
using PaintTogetherCommunicater.Contracts;
using PaintTogetherCommunicater.Messages.ClientServerCommunication;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Client;
using PaintTogetherCommunicater.Messages.PTMessageXmlSerializer;
using System.IO;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;

namespace PaintTogetherCommunicater
{
    /// <summary>
    /// Wandelt Xmls in Nachrichten und Nachrichten in Xmls um
    /// </summary>
    internal class PtMessageXmlSerializer : IPtMessageXmlSerializer
    {
        /// <summary>
        /// Das für die Kodierung und Dekodierung der XMLs verwendete
        /// Encoding
        /// </summary>
        internal static readonly Encoding UsedEncoding = Encoding.UTF8;

        /// <summary>
        /// Hier sind die Nachrichtenklassen den jeweiligen Enumwerten zugeordnet
        /// </summary>
        private static readonly Dictionary<Type, ServerClientMessageType> MessageTypes = new Dictionary<Type, ServerClientMessageType>
                     {
                         {typeof(ConnectScm),           ServerClientMessageType.ClientConnect},
                         {typeof(PaintScm),             ServerClientMessageType.ClientPaint},
                         {typeof(AllConnectionsScm),    ServerClientMessageType.ServerAllConnections},
                         {typeof(ConnectedScm),         ServerClientMessageType.ServerClientConnected},
                         {typeof(ConnectionLostScm),    ServerClientMessageType.ServerConnectionLost},
                         {typeof(NewConnectionScm),     ServerClientMessageType.ServerNewConnection},
                         {typeof(PaintContentScm),      ServerClientMessageType.ServerPaintContent},
                         {typeof(PaintedScm),           ServerClientMessageType.ServerPainted},
                     };

        /// <summary>
        /// Methodenvorschrift für Methoden zum Umwandeln einer Nachricht in eine XML
        /// </summary>
        /// <param name="toFillDocument"></param>
        /// <param name="messageNode"></param>
        /// <param name="message"></param>
        private delegate void ConvertToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message);

        /// <summary>
        /// Methodenvorschrift für Methoden zum Umwandeln einer XML in eine Nachricht
        /// </summary>
        /// <param name="toReadMessageNode"></param>
        private delegate IServerClientMessage ConvertToMessage(XmlNode toReadMessageNode);

        /// <summary>
        /// Zu jedem Nachrichtentyp gibt es eine eigene Convertermethode
        /// </summary>
        private static readonly Dictionary<ServerClientMessageType, KeyValuePair<ConvertToXml, ConvertToMessage>> XmlConverters = new Dictionary<ServerClientMessageType, KeyValuePair<ConvertToXml, ConvertToMessage>>
                {
                    {ServerClientMessageType.ClientConnect,                 new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ClientConnectMessageToXml,          ClientConnectXmlToMessage)},
                    {ServerClientMessageType.ClientPaint,                   new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ClientPaintMessageToXml,            ClientPaintXmlToMessage)},
                    {ServerClientMessageType.ServerAllConnections,          new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ServerAllConectionsMessageToXml,    ServerAllConectionsXmlToMessage)},
                    {ServerClientMessageType.ServerClientConnected,         new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ServerClientConnectedMessageToXml,  ServerClientConnectedXmlToMessage)},
                    {ServerClientMessageType.ServerConnectionLost,          new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ServerConnectionLostMessageToXml,   ServerConnectionLostXmlToMessage)},
                    {ServerClientMessageType.ServerNewConnection,           new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ServerNewConnectionMessageToXml,    ServerNewConnectionXmlToMessage)},
                    {ServerClientMessageType.ServerPaintContent,            new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ServerPaintContentMessageToXml,     ServerPaintContentXmlToMessage)},
                    {ServerClientMessageType.ServerPainted,                 new KeyValuePair<ConvertToXml, ConvertToMessage>(  
                                ServerPaintedMessageToXml,          ServerPaintedXmlToMessage)}
                };

        /// <summary>
        /// Logger fürs Logging
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtMessageReceiver");
            }
        }

        /// <summary>
        /// Verarbeitet die Aufforderung eine Nachricht in ein XML umzuwandeln
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Wenn ein unbekannter Nachrichteninhalt geschickt wird</exception>
        public void ProcessToXmlRequest(ToXmlRequest request)
        {
            request.Result = CreateXml(request.Message);
        }

        /// <summary>
        /// Erstellt aus der angegebenen Nachrichten ein XML
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string CreateXml(IServerClientMessage message)
        {
            var messageType = GetMessageType(message);
            if (messageType == ServerClientMessageType.Unknown)
            {
                throw new Exception("Der Nachrichtentyp der in XML zu kodierenden Nachricht ist unbekannt");
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(stream, UsedEncoding) { Formatting = Formatting.Indented })
                {
                    var doc = new XmlDocument();
                    var messageNode = doc.CreateNode(XmlNodeType.Element, "Message", doc.NamespaceURI);
                    var typeAttr = doc.CreateAttribute("type");
                    typeAttr.Value = Enum.GetName(messageType.GetType(), messageType);
                    messageNode.Attributes.Append(typeAttr);
                    doc.AppendChild(messageNode);

                    // Dokument durch Convertermethode füllen
                    XmlConverters[messageType].Key(doc, messageNode, message);

                    // Erstelltes Dokument jetzt über den Writer in den Stream schreiben
                    doc.WriteTo(writer);
                    writer.Flush();

                    stream.Position = 0;

                    // und mit einem Reader das XML als String auslesen
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Liefert einem zu einem Nachrichtenobjekt den Enumwert, der sich auf diesen Typen bezieht
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static ServerClientMessageType GetMessageType(IServerClientMessage message)
        {
            if (MessageTypes.ContainsKey(message.GetType()))
            {
                return MessageTypes[message.GetType()];
            }
            return ServerClientMessageType.Unknown;
        }

        /// <summary>
        /// Verarbeitet die Aufforderung ein XMl in eine Nachricht umzuwandeln
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Bei Fehlern im XML</exception>
        public void ProcessToMessageRequest(ToMessageRequest request)
        {
            try
            {
                request.Result = CreateMessage(request.Xml);
            }
            catch (Exception e)
            {
                Log.Error("Fehler bei ProcessToMessageRequest", e);
                throw;
            }
        }

        /// <summary>
        /// Erstellt aus der XML ein Nachrichtenobjekt
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static IServerClientMessage CreateMessage(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var messageNode = doc.ChildNodes[0];
            var messageTypeValue = messageNode.Attributes["type"].Value;
            doc.AppendChild(messageNode);

            var messageType = GetMessageType(messageTypeValue);

            // Dokument durch Convertermethode füllen
            return XmlConverters[messageType].Value(messageNode);
        }

        /// <summary>
        /// Liefert einem zu dem Namen einen Enumwertes den eigentlichen Enumwert
        /// </summary>
        /// <param name="messageTypeValue"></param>
        /// <returns></returns>
        private static ServerClientMessageType GetMessageType(string messageTypeValue)
        {
            return (ServerClientMessageType)Enum.Parse(typeof(ServerClientMessageType), messageTypeValue);
        }

        #region MessageToXml-Methoden
        private static void ClientConnectMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            AppendAliasNode(toFillDocument, messageNode, (message as ConnectScm).Alias);
            AppendColorNode(toFillDocument, messageNode, (message as ConnectScm).Color);
        }

        private static void ClientPaintMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            AppendColorNode(toFillDocument, messageNode, (message as PaintScm).Color);
            AppendIntAttr(toFillDocument, messageNode, "Y", (message as PaintScm).Point.Y);
            AppendIntAttr(toFillDocument, messageNode, "X", (message as PaintScm).Point.X);
        }

        private static void ServerAllConectionsMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            foreach (var painter in (message as AllConnectionsScm).Painter)
            {
                var curPainterNode = toFillDocument.CreateElement("Painter");
                AppendAliasNode(toFillDocument, curPainterNode, painter.Key);
                AppendColorNode(toFillDocument, curPainterNode, painter.Value);
                messageNode.AppendChild(curPainterNode);
            }
        }

        private static void ServerClientConnectedMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            AppendAliasNode(toFillDocument, messageNode, (message as ConnectedScm).Alias);
        }

        private static void ServerConnectionLostMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            AppendAliasNode(toFillDocument, messageNode, (message as ConnectionLostScm).Alias);
            AppendColorNode(toFillDocument, messageNode, (message as ConnectionLostScm).Color);
        }

        private static void ServerNewConnectionMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            AppendAliasNode(toFillDocument, messageNode, (message as NewConnectionScm).Alias);
            AppendColorNode(toFillDocument, messageNode, (message as NewConnectionScm).Color);
        }

        private static void ServerPaintContentMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            var bitmap = (message as PaintContentScm).PaintContent;
            var cdata = toFillDocument.CreateCDataSection(ImageToBase64String(bitmap, ImageFormat.Bmp));
            messageNode.AppendChild(cdata);
        }

        private static void ServerPaintedMessageToXml(XmlDocument toFillDocument, XmlNode messageNode, IServerClientMessage message)
        {
            AppendColorNode(toFillDocument, messageNode, (message as PaintedScm).Color);
            AppendIntAttr(toFillDocument, messageNode, "Y", (message as PaintedScm).Point.Y);
            AppendIntAttr(toFillDocument, messageNode, "X", (message as PaintedScm).Point.X);
        }
        #endregion

        #region XmlToMessage-Methoden
        private static IServerClientMessage ClientConnectXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new ConnectScm();
            result.Color = ReadColorAttr(toReadMessageNode);
            result.Alias = ReadAliasAttr(toReadMessageNode);
            return result;
        }

        private static IServerClientMessage ClientPaintXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new PaintScm();
            result.Color = ReadColorAttr(toReadMessageNode);
            result.Point = new Point(ReadIntAttr(toReadMessageNode, "X"), ReadIntAttr(toReadMessageNode, "Y"));
            return result;
        }

        private static IServerClientMessage ServerAllConectionsXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new AllConnectionsScm();
            var painterList = new List<KeyValuePair<string, Color>>();

            foreach (XmlNode curSubnode in toReadMessageNode.ChildNodes)
            {
                painterList.Add(new KeyValuePair<string, Color>(ReadAliasAttr(curSubnode), ReadColorAttr(curSubnode)));
            }

            result.Painter = painterList.ToArray();
            return result;
        }

        private static IServerClientMessage ServerClientConnectedXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new ConnectedScm();
            result.Alias = ReadAliasAttr(toReadMessageNode);
            return result;
        }

        private static IServerClientMessage ServerConnectionLostXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new ConnectionLostScm();
            result.Color = ReadColorAttr(toReadMessageNode);
            result.Alias = ReadAliasAttr(toReadMessageNode);
            return result;
        }

        private static IServerClientMessage ServerNewConnectionXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new NewConnectionScm();
            result.Color = ReadColorAttr(toReadMessageNode);
            result.Alias = ReadAliasAttr(toReadMessageNode);
            return result;
        }

        private static IServerClientMessage ServerPaintContentXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new PaintContentScm();
            result.PaintContent = ImageFromBase64String(toReadMessageNode.ChildNodes[0].Value) as Bitmap;
            return result;
        }

        private static IServerClientMessage ServerPaintedXmlToMessage(XmlNode toReadMessageNode)
        {
            var result = new PaintedScm();
            result.Color = ReadColorAttr(toReadMessageNode);
            result.Point = new Point(ReadIntAttr(toReadMessageNode, "X"), ReadIntAttr(toReadMessageNode, "Y"));
            return result;
        }
        #endregion

        #region Util-Methoden
        private static void AppendColorNode(XmlDocument toFillDocument, XmlNode messageNode, Color color)
        {
            AppendIntAttr(toFillDocument, messageNode, "Color", color.ToArgb());
        }

        private static void AppendIntAttr(XmlDocument toFillDocument, XmlNode messageNode, string name, int value)
        {
            var attr = toFillDocument.CreateAttribute(name);
            attr.Value = value.ToString();
            messageNode.Attributes.Append(attr);
        }

        private static void AppendAliasNode(XmlDocument toFillDocument, XmlNode messageNode, string alias)
        {
            var aliasAttr = toFillDocument.CreateAttribute("Alias");
            aliasAttr.Value = alias;
            messageNode.Attributes.Append(aliasAttr);
        }

        private static int ReadIntAttr(XmlNode node, string name)
        {
            return Int32.Parse(node.Attributes[name].Value);
        }

        private static Color ReadColorAttr(XmlNode toReadMessageNode)
        {
            return Color.FromArgb(Int32.Parse(toReadMessageNode.Attributes["Color"].Value));
        }

        private static string ReadAliasAttr(XmlNode toReadMessageNode)
        {
            return toReadMessageNode.Attributes["Alias"].Value;
        }

        private static string ImageToBase64String(Image image, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private static Image ImageFromBase64String(string base64)
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
            {
                return Image.FromStream(stream);
            }
        }
        #endregion
    }
}
