using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ATM.Core.Domain
{
    public class Quote
    {
        public Quote() { }
        public Quote(string instrument, string period, DateTime dateTime, double bid, double ask, double open, double close, double high, double low, double volume, double bidVolume, double askVolume)
        {
            Instrument = instrument;
            Period = period;
            DateTime = dateTime;
            Bid = bid;
            Ask = ask;
            Open = open;
            Close = close;
            High = high;
            Low = low;
            Volume = volume;
            BidVolume = bidVolume;
            AskVolume = askVolume;
        }
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Period { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double BidVolume { get; set; }
        public double AskVolume { get; set; }
        public string Instrument { get; set; }
        public string ToXml()
        {
            var template = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Quote xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <DateTime>{0}</DateTime>
  <Period>{1}</Period>
  <Open>{2}</Open>
  <Close>{3}</Close>
  <High>{4}</High>
  <Low>{5}</Low>
  <Volume>{6}</Volume>
  <Bid>{7}</Bid>
  <Ask>{8}</Ask>
  <BidVolume>{9}</BidVolume>
  <AskVolume>{10}</AskVolume>
  <Instrument>{11}</Instrument>  
</Quote>";
            var result = String.Format(template, DateTime.ToString("o"), Period, Open, Close, High, Low, Volume, Bid, Ask, BidVolume, AskVolume, Instrument);
            return result;
        }

        public static Quote FromXmlString(string xmlString)
        {
            var doc = XDocument.Parse(xmlString);
            var result = new Quote();
            result.Ask = double.Parse(doc.Root.Element("Ask").Value);
            result.AskVolume = double.Parse(doc.Root.Element("AskVolume").Value);
            result.Bid = double.Parse(doc.Root.Element("Bid").Value);
            result.BidVolume = double.Parse(doc.Root.Element("BidVolume").Value);
            result.Close = double.Parse(doc.Root.Element("Close").Value);
            result.DateTime = DateTime.Parse(doc.Root.Element("DateTime").Value);
            result.High = double.Parse(doc.Root.Element("High").Value);
            result.Instrument = doc.Root.Element("Instrument").Value;
            result.Low = double.Parse(doc.Root.Element("Low").Value);
            result.Open = double.Parse(doc.Root.Element("Open").Value);
            result.Period = doc.Root.Element("Period").Value;
            result.Volume = double.Parse(doc.Root.Element("Volume").Value);
            return result;
        }
    }

}
