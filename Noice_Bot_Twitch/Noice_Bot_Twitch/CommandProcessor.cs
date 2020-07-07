﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

//Processing a given command like changing the username, checking black and whitelisting
namespace Noice_Bot_Twitch
{
    class CommandProcessor
    {
        FileManager fm;
        private int _maxTextLength = 200;
        public int MaxTextLength
        {
            get { return _maxTextLength; }
            set { _maxTextLength = value; }
        }
        private int _spamThreshold = 30;
        public int BadThreshold
        {
            get { return _spamThreshold; }
            set { _spamThreshold = value; }
        }

        //Init
        public CommandProcessor(FileManager fm)
        {
            this.fm = fm;
            _maxTextLength = fm.GetMaxTextLength();
            _spamThreshold = fm.GetSpamThreshold();
        }

        //Checks the Username and replaces it with an Alias of the list
        public Comment CheckAlias(Comment c)
        {
            foreach (string s in fm.GetAliasList())
            {
                string username = s.Substring(0, s.IndexOf(","));
                string alias = s.Substring(s.IndexOf(",")+1);


                if (c.user == username.ToLower())
                {
                    c.user = alias.ToLower();
                }
            }
            return c;
        }
        
        //Check if someone is spamming a lot of waird symboles and removes the command
        //Checks the length of a command and reduces it to a given length
        public Comment SpamProtection(Comment c)
        {
            //Cut the comment down
            if(c.comment.Length > _maxTextLength) c.comment = c.comment.Substring(0, _maxTextLength);

            if (fm.GetRemoveEmojis()) 
            {
                c.comment = Regex.Replace(c.comment, @"\p{Cs}", ""); //Remove all unicode emojis if true
                if (c.comment == "") //If the comment where just unicode emojis say "unicode emoji"
                {
                    c.comment = "unicode Emoji";
                }
            }

            string badStuff = @fm.GetBadCharList(); //Remove unwanted spamming of specific symboles
            int badCounter = 0;
            foreach(char comChar in c.comment)
            {
                foreach(char badChar in badStuff)
                {
                    if (comChar == badChar) badCounter++;
                }
                if(badCounter >= _spamThreshold) c.comment = "";
            }
            return c;
        }

        //Checks if a user is on the Blacklist of TTS
        //Return true if he is on the list, otherwise false
        public bool CheckBlacklist(Comment c)
        {
            foreach (string s in fm.GetBlackList())
            {
                string username = s.ToLower();

                if (c.user == username.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //Checks if a user is on the Whitelist of controlling the Bot
        //Return true if he is on the list, otherwise false
        public bool CheckWhiteList(Comment c)
        {
            foreach (string s in fm.GetWhiteList())
            {
                string username = s.ToLower();

                if (c.user == username.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //Remove numbers in usernames for faster reading
        public String RemoveNumeric(string text)
        {
            string textwithoutnumeric = new String(text.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
            return (textwithoutnumeric);
        }
    }
}
