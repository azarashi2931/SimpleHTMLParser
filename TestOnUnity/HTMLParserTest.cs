using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Azarashi.Utilities.HTML;

namespace Tests
{
    public class HTMLParserTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void HTMLParserTestSimplePasses()
        {
            string source = @"<!-- 
  Using Modernizr.js for smartphone.
  http://modernizr.com
-->
<div class='general-button'>
  <div class='button-content'>
    <span class='icon-font'>file</span>
    <span class='button-text'>TEXT</span>
  </div>
</div>
";

            SetLogAssertExpect("div", "class", "general-button");
            SetLogAssertExpect("div", "class", "button-content");
            SetLogAssertExpect("span", "class", "icon-font");
            SetLogAssertExpect("span", "class", "button-text");
            
            HTMLParser parser = new HTMLParser(source);
            TagList tagList = parser.Parse();
            foreach (Tag tag in tagList)
                foreach (KeyValuePair<string, Attribute> item in tag)
                    Debug.Log("TagName : " + tag.Name +
                        " AttributeName : " + item.Value.Name +
                        " AttributeValue : " + item.Value.Value);
            
        }

        void SetLogAssertExpect(string tagName,string AttributeName,string AttributeValue)
        {
            LogAssert.Expect(LogType.Log, 
                        "TagName : " + tagName +
                        " AttributeName : " + AttributeName +
                        " AttributeValue : " + AttributeValue);
        }
    }
}
