using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Assets.Scripts
{
	public class CreateCardByXml 
	{
		#if UNITY_EDITOR
		public static string Path = Application.dataPath + @"/Xml/Wixoss_Project.xml";
		#endif

		public static XElement LoadFromXml()
		{
			XElement root = XElement.Load (Path);
			return root;
		}

		/// <summary>
		/// 通过cardid来获取card的信息
		/// </summary>
		/// <returns>The card by card identifier.</returns>
		/// <param name="cardid">Cardid.</param>
		public static IEnumerable<XElement> GetCardByCardId(string cardid)
		{
			var root = LoadFromXml ();
			var element = root.Elements ();
			var card = from i in element where i.Element ("CardId").Value == cardid select i;
			return card;
		}
	}
}
