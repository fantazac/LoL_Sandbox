using System.Globalization;

public static class ExtensionMethods
{
    private static readonly CultureInfo culture = new CultureInfo("en-US");

    private static string ToDisplay(this float value)
    {
        string stringValue = value.ToString(culture);
        if (stringValue.Contains("."))
        {
            stringValue += "f";
        }

        return stringValue;
    }

    public static string[] ToDisplay(this float[] values)
    {
        string[] newValues = new string[18];
        for (int i = 0; i < values.Length; i++)
        {
            newValues[i] = values[i].ToDisplay();
        }

        return newValues;
    }
}
