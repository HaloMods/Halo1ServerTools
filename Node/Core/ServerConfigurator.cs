/* ---------------------------------------------------------------
 * The Hive
 * Server Hosting Management System
 * (c)2005, EPG Servers, LLC.
 * ---------------------------------------------------------------
 * Author      : Justin Draper
 * Co-Authors  : None
 * ---------------------------------------------------------------
 * This code is protected under the copyright laws of the
 * United States of America, and is confidential.  Use or
 * posession of this code without express written consent of
 * EPG Servers, LLC. is expressly forbidden.
 * ---------------------------------------------------------------
 */

using System;
using System.Collections;
using System.IO;

namespace Hive.Node.Core
{
  /// <summary>
  /// Summary description for ServerConfigurator.
  /// </summary>
  public abstract class ServerConfigurationDefinition
  {
    protected ConfigurationValue[] values = new ConfigurationValue[4];
    
    public ServerConfigurationDefinition() { ; }
  }

  public class HaloServerConfigurationDefinition : ServerConfigurationDefinition
  {
    public HaloServerConfigurationDefinition()
    {
      values[0] = new ConfigurationValue("sv_name", "Server Name", 
        "Sets the name that will identify your server in the GameSpy(tm) server list within Halo.",
        new VariableParameter("Name", "Alpha-numeric and punctuation allowed - 32 characters maximum.",
        ValueType.Text, null,
        new StringParameterValidator(32, CharacterSets.AlphabetAndPunctuation)
      ));

      values[1] = new ConfigurationValue("sv_public", "Server Visibility",
        "Specifies if the server will be displayed in the GameSpy(tm) server list.",
        new VariableParameter("Value", "0 = False, 1 = True",
        ValueType.Number,
        new VariableParameter.ValueCollection(0, 1), 
        new NumberParameterValidator(0, 1)
      ));

      values[2] = new ConfigurationValue("sv_maxplayers", "Player Count",
        "The maximum number of players allowed in the server.",
        new VariableParameter("Value", "",
        ValueType.Number,
        new VariableParameter.ValueCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), 
        new NumberParameterValidator(1, 16)
      ));
      
      values[3] = new ConfigurationValue("sv_rcon_password", "RCON Password",
        "The password used to issue console commands to the server.",
        new VariableParameter("Password", "",
        ValueType.Text,
        null,
        new StringParameterValidator(8, CharacterSets.AlphaNumericOnly)
      ));
    }
  }
  
  /// <summary>
  /// Represents a value contained within a server configuration file.
  /// </summary>
  public class ConfigurationValue
  {
    private string key; // ex: sv_map_add
    private string friendlyName;
    private string description;
    private VariableParameter[] parameters;

    public string Key
    {
      get { return key; }
    }

    public string FriendlyName
    {
      get { return friendlyName; }
    }

    public string Description
    {
      get { return description; }
    }

    public VariableParameter[] Parameters
    {
      get { return parameters; }
    }

    #region Constructor
    public ConfigurationValue(string key, string friendlyName, string description, params VariableParameter[] parameters)
    {
      this.key = key;
      this.friendlyName = friendlyName;
      this.description = description;
      this.parameters = parameters;
    }
    #endregion
  }

  public enum ValueType
  {
    Text,
    Number
  }

  /// <summary>
  /// A parameter that is used by a ConfigurationValue.
  /// </summary>
  public class VariableParameter
  {
    private string name;
    private string description;
    private ValueType valueType;
    private ValueCollection values;
    private IParameterValidator[] validators;  

    /// <summary>
    /// The name of the parameter.
    /// </summary>
    public string Name
    {
      get { return name; }
    }

    /// <summary>
    /// A text description of the parameter.
    /// </summary>
    public string Description
    {
      get { return description; }
    }

    public VariableParameter(string name, string description, ValueType valueType,
      ValueCollection values, params IParameterValidator[] validators)
    {
      this.name = name;
      this.description = description;
      this.valueType = valueType;
      this.values = values;
      this.validators = validators;
    }

    /// <summary>
    /// Represents a collection of default values to be used by a VariableParamter.
    /// </summary>
    public class ValueCollection
    {
      private object[] values;
      
      /// <summary>
      /// The array of values contained in this collection.
      /// </summary>
      public object[] Values
      {
        get { return values; }
      }

      public ValueCollection(params object[] values)
      {
        this.values = values;
      }
    }
  }

  /// <summary>
  /// Contains static arrays of predefined character sets.
  /// </summary>
  public class CharacterSets
  {
    private static char[] alphabetAndPunctuation;
    private static char[] alphaNumericOnly;
    
    /// <summary>
    /// Returns an array containing all alphabet and punctuation characters.
    /// ASCII codes 32 to 126.
    /// </summary>
    public static char[] AlphabetAndPunctuation
    {
      get
      {
        FileStream f = new FileStream("this", FileMode.OpenOrCreate);
        if (alphabetAndPunctuation == null)
        {
          // Create the alphabetAndPunctuation array
          // ASCII codes 32 to 126
          byte[] bin = new byte[95];
          for (int x=32; x<=126; x++)
          {
            bin[x] = (byte)(x-32);
          }
          alphabetAndPunctuation = System.Text.Encoding.ASCII.GetChars(bin);
        }
        return alphabetAndPunctuation;
      }
    }

    /// <summary>
    /// Returns an array containing all alphabet and punctuation characters.
    /// ASCII codes 48 to 57, 65 to 90, and  97 to 122.
    /// </summary>
    public static char[] AlphaNumericOnly
    {
      get
      {
        if (alphaNumericOnly == null)
        {
          // Create the alphaNumericOnly array
          // ASCII codes 48 to 57 (10), 65 to 90 (26), and  97 to 122 (26).
          byte[] bin = new byte[62] {
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
            65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90,
            97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122
          };
          alphaNumericOnly = System.Text.Encoding.ASCII.GetChars(bin);
        }
        return alphaNumericOnly;
      }
    }
  }

  /// <summary>
  /// Provides a base for creating validation object for parameters.
  /// </summary>
  public interface IParameterValidator
  {
    /// <summary>
    /// When overridden in a child class, validates the given parameter, throwing an exception
    /// if the parameter is found to be invalid.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    void Validate(object value);  
  }

  /// <summary>
  /// Validates a given string based on the specified criteria.
  /// </summary>
  public class StringParameterValidator : IParameterValidator
  {
    private int maxLength;
    private char[] allowedCharacters;
    
    public StringParameterValidator(int maxLength, char[] allowedCharacters)
    {
      this.maxLength = maxLength;
      this.allowedCharacters = allowedCharacters;
    }

    /// <summary>
    /// Validates the supplied string.  Throws an error if the string is not valid.
    /// </summary>
    /// <param name="value">The object to validate - must be a string.</param>
    public void Validate(object value)
    {
      if (!(value is string))
      {
        throw new ParameterNotValidException(
          "StringParameterValidator cannot be used on an object of type '" + value.GetType().ToString() + "'");
      }
      
      string s = (string)value;
      if (s.Length > this.maxLength)
      {
        throw new ParameterNotValidException(String.Format(
          "The string {0} ({1} characters) exceeds the maximum length of {2} characters.", s, s.Length, maxLength));
      }

      string allowed = new string(allowedCharacters);      
      foreach (char c in s.ToCharArray())
      {
        // Ensure that the "allowed" array contains this character from the string being tested.
        if (allowed.IndexOf(c) < 0)
          throw new ParameterNotValidException("The string '" + s + "'contains one or more invalid characters.");
      }
    }
  }

  /// <summary>
  /// Validates a given number based on the specified criteria.
  /// </summary>
  public class NumberParameterValidator : IParameterValidator
  {
    private int min;
    private int max;

    public NumberParameterValidator(int min, int max)
    {
      this.min = min;
      this.max = max;
    }

    /// <summary>
    /// Validates the supplied int.  Throws an error if the int is not valid.
    /// </summary>
    /// <param name="value">The object to validate - must be an int.</param>
    public void Validate(object value)
    {
      if (!(value is int))
      {
        throw new ParameterNotValidException(
          "NumberParameterValidator cannot be used on an object of type '" + value.GetType().ToString() + "'");
      }
      
      int i = (int)value;
      if ((i > min) || (i < max))
      {
        throw new ParameterNotValidException(String.Format(
          "The number specified ({0}) must be between {1} and {2}", i, min, max));
      }
    }
  }


  /// <summary>
  /// An error that is thrown when a parameter is found to be invalid.
  /// </summary>
  public class ParameterNotValidException : Exception
  {
    public ParameterNotValidException(string message) : base(message) { ; }
  }
}
