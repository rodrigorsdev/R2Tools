namespace R2.Tools.Mask
{
    using R2.Tools.Mask.Enum;

    public class MaskTool
    {
        public static string RemoveMask(string value)
        {
            string resultado = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("/", string.Empty);
                value = value.Replace("-", string.Empty);
                value = value.Replace(".", string.Empty);
                value = value.Replace("(", string.Empty);
                value = value.Replace(")", string.Empty);
                value = value.Replace(" ", string.Empty);
                value = value.Trim();

                resultado = value;
            }

            return resultado;
        }

        public static string InsertMask(string value, MaskType type)
        {
            string resultado = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                switch (type)
                {
                    case (MaskType.Tel):

                        resultado = "(" + value.Substring(0, 2) + ") ";

                        if (value.Length == 10)
                        {
                            resultado += value.Substring(2, 4) + '-';
                            resultado += value.Substring(6, 4);
                        }
                        else if (value.Length == 11)
                        {
                            resultado += value.Substring(2, 5) + '-';
                            resultado += value.Substring(7, 4);
                        }
                        break;
                    case (MaskType.CEP):
                        resultado = value.Substring(0, 5) + '-';
                        resultado += value.Substring(5, 3);
                        break;
                    case (MaskType.CNPJ):
                        resultado = value.Substring(0, 2) + '.';
                        resultado += value.Substring(2, 3) + '.';
                        resultado += value.Substring(5, 3) + '/';
                        resultado += value.Substring(8, 4) + '-';
                        resultado += value.Substring(12);
                        break;
                    case (MaskType.CPF):
                        resultado = value.Substring(0, 3) + '.';
                        resultado += value.Substring(3, 3) + '.';
                        resultado += value.Substring(6, 3) + '-';
                        resultado += value.Substring(9, 2);
                        break;
                    case (MaskType.CurrencyRealBrasil):
                        resultado = string.Format(System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), "R$ {0:#,###.##}", value);
                        break;
                    case (MaskType.CurrencyRealBrasilWithoutSymbol):
                        resultado = string.Format(System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), "{0:#,###.##}", value);
                        break;
                }
            }

            return resultado.Trim();
        }
    }
}
