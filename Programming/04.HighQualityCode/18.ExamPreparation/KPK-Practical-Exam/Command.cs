﻿using System;
using System.Linq;

namespace CatalogOfFreeContent
{
    public class Command : ICommand
    {
        readonly char[] paramsSeparators = { ';' };
        readonly char commandEnd = ':';

        public CommandType Type { get; set; }

        public string OriginalForm { get; set; }

        public string Name { get; set; }

        public string[] Parameters { get; set; }

        private int commandNameEndIndex;

        public Command(string input)
        {
            this.OriginalForm = input.Trim();

            this.Parse();
        }

        private void Parse()
        {
            this.commandNameEndIndex = this.GetCommandNameEndIndex();

            this.Name = this.ParseName();
            this.Parameters = this.ParseParameters();
            this.TrimParams();

            this.Type = this.ParseCommandType(this.Name);
        }

        public CommandType ParseCommandType(string commandName)
        {
            if (commandName.Contains(':') || commandName.Contains(';'))
            {
                throw new FormatException("Invalid command. Commands cannot contain ':' and ';'");
            }

            switch (commandName.Trim())
            {
                case "Add book":
                    return CommandType.AddBook;
                case "Add movie":
                    return CommandType.AddMovie;
                case "Add song":
                    return CommandType.AddSong;
                case "Add application":
                    return CommandType.AddApplication;
                case "Update":
                    return CommandType.Update;
                case "Find":
                    return CommandType.Find;
                default:
                    throw new ArgumentException("Invalid command: " + commandName);
            }
        }

        public string ParseName()
        {
            string name = this.OriginalForm.Substring(0, this.commandNameEndIndex + 1);
            return name;
        }

        public string[] ParseParameters()
        {
            int paramsLength =
                this.OriginalForm.Length - (this.commandNameEndIndex + 3);

            string paramsOriginalForm = this.OriginalForm.Substring(
                this.commandNameEndIndex + 3, paramsLength);

            string[] parameters = paramsOriginalForm.Split(
                paramsSeparators, StringSplitOptions.RemoveEmptyEntries);

            return parameters;
        }

        public int GetCommandNameEndIndex()
        {
            int endIndex = this.OriginalForm.IndexOf(commandEnd) - 1;

            return endIndex;
        }

        private void TrimParams()
        {
            for (int i = 0; i < this.Parameters.Length; i++)
            {
                this.Parameters[i] = this.Parameters[i].Trim();
            }
        }
    }
}
