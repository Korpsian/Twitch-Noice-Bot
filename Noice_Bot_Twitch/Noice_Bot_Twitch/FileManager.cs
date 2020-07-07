﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace Noice_Bot_Twitch
{
    //This class Manages all Files the bot need to operate
    class FileManager
    {
        //Path and Filenames of every used file
        string path;
        string settingsFile = @"Settings.txt";
        string aliasFile = @"Aliaslist.txt";
        string blacklistFile = @"Blacklist.txt";
        string bridgelistFile = @"BridgeWordList.txt";
        string whitelistFile = @"Whitelist.txt";

        //Folder structure
        string soundEffectsFolder = @"Soundeffects";
        string notificationSoundsFolder = @"Notifications";
        string soundBoardFolder = @"Soundboard";

        //Alias List, Blacklist, Whitelist, Settings
        List<String> aliasList;
        List<String> blackList;
        List<String> bridgeWordList;
        List<String> whiteList;
        List<String> settingsList;
        List<String> notificationList; //Paths to the notifications sounds
        List<String> soundboardList; //Paths to the soundboard sounds

        public FileManager()
        {
            path = Directory.GetCurrentDirectory(); //Get the current execution Directory
            CheckExistence(); //Check file existance of all needed files, create missing ones (with examples)
            LoadFiles(); //Load the files into a string list object
        }

        public void LoadFiles() //Load all the Files
        {
            try //If an error occures while trying to load a file check the existence
            {
                notificationList = Directory.GetFiles(path + @"\" + soundEffectsFolder + @"\" + notificationSoundsFolder).ToList();

                aliasList = File.ReadAllLines(path + @"\" + aliasFile).ToList();
                blackList = File.ReadAllLines(path + @"\" + blacklistFile).ToList();
                bridgeWordList = File.ReadAllLines(path + @"\" + bridgelistFile).ToList();
                whiteList = File.ReadAllLines(path + @"\" + whitelistFile).ToList();
                settingsList = File.ReadAllLines(path + @"\" + settingsFile).ToList();
            } catch
            {
                CheckExistence(); //Check existance
                LoadFiles(); //Load files again
            }
        }
        void CheckExistence() //Does all the wanted files exist? No? Then Create them and put examples in it
        {
            if(!File.Exists(path + @"\" + aliasFile)) //Alislist.txt
            {
                File.WriteAllText(path + @"\" + aliasFile, GenAliasFile());
                Console.WriteLine("File: " + aliasFile + " was missing");
            }
            if (!File.Exists(path + @"\" + blacklistFile)) //Blacklist.txt
            {
                string str = "USER1\nuser2\nuSeR3"; //Example
                File.WriteAllText(path + @"\" + blacklistFile, GenBlacklisteFile());
                Console.WriteLine("File: " + blacklistFile + " was missing");
            }
            if (!File.Exists(path + @"\" + bridgelistFile)) //Bridgelist.txt
            {
                string str = "says\nsay\ntells";
                File.WriteAllText(path + @"\" + bridgelistFile, GenBridgeListFile());
                Console.WriteLine("File: " + bridgelistFile + " was missing");
            }
            if (!File.Exists(path + @"\" + whitelistFile)) //Whitelist.txt
            {
                string str = "USER1\nuser2\nuSeR3"; //Example
                File.WriteAllText(path + @"\" + whitelistFile, GenWhitelistFile());
                Console.WriteLine("File: " + whitelistFile + " was missing");
            }
            if (!File.Exists(path + @"\" + settingsFile)) //Settings.txt
            {
                //Basic Conf, the user has to put in his own oauth key and channelname
                File.WriteAllText(path + @"\" + settingsFile, GenSettingsFile());
                Console.WriteLine("File: " + settingsFile + " was missing");
            }

            //Create Folder Structure for notifications and the soundboard
            if(!Directory.Exists(path + @"\" + soundEffectsFolder))
            {
                Directory.CreateDirectory(path + @"\" + soundEffectsFolder);
            }
            if (!Directory.Exists(path + @"\" + soundEffectsFolder + @"\" + notificationSoundsFolder))
            {
                Directory.CreateDirectory(path + @"\" + soundEffectsFolder + @"\" + notificationSoundsFolder);
            }
            if (!Directory.Exists(path + @"\" + soundEffectsFolder + @"\" + soundBoardFolder))
            {
                Directory.CreateDirectory(path + @"\" + soundEffectsFolder + @"\" + soundBoardFolder);
            }
        
        }

        //File Generation
        String GenAliasFile()
        {
            string str = @"
Korpsian,the developer
User1,CoolKid
USER2,NiceDude
UsEr3,TrollyDude
";
            return str;
        }

        String GenBlacklisteFile()
        {
            string str = @"
YourBot
TrollKid69420
HahaFunnyGuy1
User1
user2
USER3
";
            return str;
        }

        String GenBridgeListFile()
        {
            string str = @"
says
say
speaks
writes
";
            return str;
        }

        String GenWhitelistFile()
        {
            string str = @"
User1
USER2
YourMods
Yourself
";
            return str;
        }

        String GenSettingsFile()
        {
            string str = @"
#See the GitHub Page for how to use this file correctly
#https://github.com/Korpsian/Twitch-Noice-Bot

--IrcClient Settings--
ircclient=irc.twitch.tv
port=6667
botname=noisebot
channelname=
oauth=oauth:
            
--Notification Settings--
ttsbasespeed=3
ttsmaxspeed=7
#n = notification sound, u = username, b = bridgeword, c = comment
notificationExecutionOrder=ubc

--Anti Spam Settings--
maxtextlength=100
spamthreshold=8
removeemojis=true
badcharlist=!§$%&/()=?`^\{[]}#

--Audio Device Settings--
ttsoutputdevice=
soundboardoutputdevice=
notificationoutputdevice=
notificationVolume=0,5
ttsVolume=1

";
            return str;
        }
        //File Generation


        //Getter methos for general path, alias, blacklist, whitelist and soundfile
        public string GetPath()
        {
            return path;
        }
        public string GetAliasFile()
        {
            return aliasFile;
        }
        public string GetBlacklistFile()
        {
            return blacklistFile;
        }
        public string GetWhitelistFile()
        {
            return whitelistFile;
        }

        //Get Specific Settings out of the Settings.txt
        public string GetIrcClient()
        {
            foreach(string s in settingsList)
            {
                if(s.Contains("ircclient=") && s.Length > 10)
                {
                    return s.Substring(s.IndexOf("=") + 1);
                }
            }
            Console.WriteLine("INCORRECT IRC CLIENT DETECTED");
            return null;
        } //IRC Settings
        public int GetPort()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("port=") && s.Length > 5)
                {
                    int i = -1;
                    if(int.TryParse(s.Substring(s.IndexOf("=")+1), out i))
                    {
                        return i;
                    }
                }
            }
            Console.WriteLine("INCORRECT PORT DETECTED");
            return 0;
        }
        public string GetBotName()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("botname=") && s.Length > 8)
                {
                    return s.Substring(s.IndexOf("=") + 1);
                }
            }
            Console.WriteLine("INCORRECT BOT NAME DETECTED");
            return null;
        }
        public string GetChannelName()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("channelname=") && s.Length > 12)
                {
                    return s.Substring(s.IndexOf("=") + 1);
                }
            }
            Console.WriteLine("INCORRECT CHANNELNAME DETECTED");
            return null;
        }
        public string GetOAuth()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("oauth=") && s.Length > 12)
                {
                    return s.Substring(s.IndexOf("=") + 1);
                }
            }
            Console.WriteLine("INCORRECT OAUTH DETECTED");
            return null;
        } //IRC Settings
        public int GetTTSBaseSpeed()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("ttsbasespeed=") && s.Length > 13)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return -11;
        }//TTS Settings
        public int GetTTSMaxSpeed()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("ttsmaxspeed=") && s.Length > 12)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return -11;
        } //TTS Settings
        public int GetMaxTextLength() //Anti Spam
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("maxtextlength=") && s.Length > 14)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        public int GetSpamThreshold()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("spamthreshold=") && s.Length > 14)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return 0;
        } //Anti Spam
        public bool GetRemoveEmojis()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("removeemojis=") && s.Length > 13)
                {
                    string str = s.Substring(s.IndexOf("=") + 1);
                    if (str.Contains("true")) return true;
                    if (str.Contains("false")) return false;
                }
            }
            return false;
        }
        public string GetBadCharList()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("badcharlist=") && s.Length > 12)
                {
                    return s.Substring(s.IndexOf("=") + 1);
                }
            }
            return "";
        } //IRC Settings
        public int GetTTSOutputDeviceID()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("ttsoutputdevice=") && s.Length > 16)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return -2;
        } //Audio Device Settings
        public int GetSoundboardOutputDeviceID()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("soundboardoutputdevice=") && s.Length > 23)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return -2;
        }
        public int GetNotificationOutputDeviceID()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("notificationoutputdevice=") && s.Length > 25)
                {
                    int i = -2;
                    if (int.TryParse(s.Substring(s.IndexOf("=") + 1), out i))
                    {
                        return i;
                    }
                }
            }
            return -2;
        } //Audio Device Settings
        public string GetNotificationExecutionOrder()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("notificationExecutionOrder=") && s.Length > 27)
                {
                    return s.Substring(s.IndexOf("=") + 1);
                }
            }
            Console.WriteLine("INCORRECT NOTIFICATION EXECUTION ORDER DETECTED");
            return null;
        } //IRC Settings

        public float GetNotificationVolume()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("notificationVolume=") && s.Length > 19)
                {
                    float f = 0f;
                    if (float.TryParse(s.Substring(s.IndexOf("=") + 1), out f))
                    {
                        return f;
                    }
                }
            }
            return 0.5f;
        }
        public float GetTTSVolume()
        {
            foreach (string s in settingsList)
            {
                if (s.Contains("ttsVolume=") && s.Length > 10)
                {
                    float f = 0f;
                    if (float.TryParse(s.Substring(s.IndexOf("=") + 1), out f))
                    {
                        return f;
                    }
                }
            }
            return 0.5f;
        }


        //Return the created String lists
        public List<String> GetBlackList()
        {
            return blackList;
        }
        public List<String> GetBridgeWordList()
        {
            return bridgeWordList;        }
        public List<String> GetAliasList()
        {
            return aliasList;
        }
        public List<String> GetWhiteList()
        {
            return whiteList;
        }

        public String GetRandomNotificationSound()
        {
            Random rand = new Random();
            string str = notificationList[rand.Next(notificationList.Count)];
            return str;
        }

        //Take a random bridge word out of Brideword.txt and return it
        public String GetRandomBridgeWord() 
        {
            Random rand = new Random();
            string str = bridgeWordList[rand.Next(bridgeWordList.Count)];
            return str;
        }
    }
}
