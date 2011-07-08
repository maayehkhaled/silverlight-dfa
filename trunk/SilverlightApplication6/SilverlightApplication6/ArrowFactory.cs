using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using PathConverter;

namespace SilverlightApplication6
{
	/** */
	public enum ArrowType
	{
		Arrow_27
	}
	public class ArrowFactory
	{
		private static Dictionary<ArrowType, string> templates;
		private static StringToPathGeometryConverter pconverter;

		static ArrowFactory()
		{
			/* create new Dictionary */
			templates = new Dictionary<ArrowType, string>();

			templates.Add(ArrowType.Arrow_27,
				//"M 100,200 C 100,25 400,350 400,175 H 280"
				"F1 M 180.758,724.259L 170.642,724.259L 182.306,712.592L 159.381,712.592L 159.381,705.168L 182.226,705.168L 170.622,693.563L 180.758,693.58L 196.097,708.919M 149.432,737.219L 206.046,737.219L 206.046,680.604L 149.432,680.604L 149.432,737.219 Z "
				);
			/* copy some more here if nesccesary */
			/* use pathconverter */
			pconverter = new StringToPathGeometryConverter();
		}

		public static Geometry createArrow(ArrowType arrow)
		{
			if (templates.ContainsKey(arrow) )
			{
	
				return pconverter.Convert(templates[arrow]);;
			}else
			{
				throw new Exception("Arrow template not found");
			}
		}
	}
}
