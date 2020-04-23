﻿using System.Xml;

namespace CSharp.Framework.Helper
{
    public static class XmlHelper
    {
        //todo 根据字符串获取节点信息 Root.Selction.Item


        public static string GetNodeString(string nodeString)
        {
            var doc = new XmlDocument();
            doc.Load("");

            var selectNodes=doc.SelectNodes(nodeString);
            
            return null;
        }
    }
}