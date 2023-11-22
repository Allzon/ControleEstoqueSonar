using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rotativa.Options
{
    public class Margins
    {
        /// <summary>
        /// Page bottom margin in mm.
        /// </summary>
        [OptionFlag("-B")] private int? bottom;

        /// <summary>
        /// Page left margin in mm.
        /// </summary>
        [OptionFlag("-L")] private int? left;

        /// <summary>
        /// Page right margin in mm.
        /// </summary>
        [OptionFlag("-R")] private int? right;

        /// <summary>
        /// Page top margin in mm.
        /// </summary>
        [OptionFlag("-T")] private int? top;

        public int? Bottom { get => bottom; set => bottom = value; }
        public int? Left { get => left; set => left = value; }
        public int? Right { get => right; set => right = value; }
        public int? Top { get => top; set => top = value; }

        public Margins()
        {
        }

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        /// <param name="top">Page top margin in mm.</param>
        /// <param name="right">Page right margin in mm.</param>
        /// <param name="bottom">Page bottom margin in mm.</param>
        /// <param name="left">Page left margin in mm.</param>
        public Margins(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo fi in fields)
            {
                var of = fi.GetCustomAttributes(typeof(OptionFlagAttribute), true).FirstOrDefault() as OptionFlagAttribute;
                if (of == null)
                    continue;

                object value = fi.GetValue(this);
                if (value != null)
                    result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
            }

            return result.ToString().Trim();
        }
    }
}