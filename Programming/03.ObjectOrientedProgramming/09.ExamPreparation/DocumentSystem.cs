﻿using System;
using System.Collections.Generic;
using System.Linq;

public class DocumentSystem
{
    private static List<Document> allDocuments = new List<Document>();

    static void Main()
    {
        // I have made a text file named input.txt where I have copied the input info
        // so that there is no need to enter it on the Console every time I want to test the program
        // Just when click Ctrl + F5 and it reads the input from this file and shows the result on the Console
        // the following code reads the information from input.txt  
//#if DEBUG
//        Console.SetIn(new System.IO.StreamReader("../../input.txt"));
//#endif

        IList<string> allCommands = ReadAllCommands();
        ExecuteCommands(allCommands);
    }

    private static IList<string> ReadAllCommands()
    {
        List<string> commands = new List<string>();
        while (true)
        {
            string commandLine = Console.ReadLine();
            if (commandLine == "")
            {
                // End of commands
                break;
            }
            commands.Add(commandLine);
        }
        return commands;
    }

    private static void ExecuteCommands(IList<string> commands)
    {
        foreach (var commandLine in commands)
        {
            int paramsStartIndex = commandLine.IndexOf("[");
            string cmd = commandLine.Substring(0, paramsStartIndex);
            int paramsEndIndex = commandLine.IndexOf("]");
            string parameters = commandLine.Substring(
                paramsStartIndex + 1, paramsEndIndex - paramsStartIndex - 1);
            ExecuteCommand(cmd, parameters);
        }
    }

    private static void ExecuteCommand(string cmd, string parameters)
    {
        string[] cmdAttributes = parameters.Split(
            new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        if (cmd == "AddTextDocument")
        {
            AddTextDocument(cmdAttributes);
        }
        else if (cmd == "AddPDFDocument")
        {
            AddPdfDocument(cmdAttributes);
        }
        else if (cmd == "AddWordDocument")
        {
            AddWordDocument(cmdAttributes);
        }
        else if (cmd == "AddExcelDocument")
        {
            AddExcelDocument(cmdAttributes);
        }
        else if (cmd == "AddAudioDocument")
        {
            AddAudioDocument(cmdAttributes);
        }
        else if (cmd == "AddVideoDocument")
        {
            AddVideoDocument(cmdAttributes);
        }
        else if (cmd == "ListDocuments")
        {
            ListDocuments();
        }
        else if (cmd == "EncryptDocument")
        {
            EncryptDocument(parameters);
        }
        else if (cmd == "DecryptDocument")
        {
            DecryptDocument(parameters);
        }
        else if (cmd == "EncryptAllDocuments")
        {
            EncryptAllDocuments();
        }
        else if (cmd == "ChangeContent")
        {
            ChangeContent(cmdAttributes[0], cmdAttributes[1]);
        }
        else
        {
            throw new InvalidOperationException("Invalid command: " + cmd);
        }
    }

    private static void AddDocument(Document document, string[] attributes)
    {
        foreach (var attribute in attributes)
        {
            string[] result = attribute.Split('=');
            document.LoadProperty(result[0], result[1]);
        }

        if (document.Name == null)
        {
            Console.WriteLine("Document has no name");
        }

        else
        {
            Console.WriteLine("Document added: " + document.Name);
            
            allDocuments.Add(document);
        }
    }

    private static void AddTextDocument(string[] attributes)
    {
        AddDocument(new TextDocument(), attributes);
    }

    private static void AddPdfDocument(string[] attributes)
    {
        AddDocument(new PDFDocument(), attributes);
    }

    private static void AddWordDocument(string[] attributes)
    {
        AddDocument(new WordDocument(), attributes);
    }

    private static void AddExcelDocument(string[] attributes)
    {
        AddDocument(new ExcelDocument(), attributes);
    }

    private static void AddAudioDocument(string[] attributes)
    {
        AddDocument(new AudioDocument(), attributes);
    }

    private static void AddVideoDocument(string[] attributes)
    {
        AddDocument(new VideoDocument(), attributes);
    }

    private static void ListDocuments()
    {
        if (allDocuments.Count == 0)
        {
            Console.WriteLine("No documents found");
            return;
        }

        allDocuments.ForEach(doc => Console.WriteLine(doc));
    }

    private static void EncryptDocument(string name)
    {
        Document[] docs = allDocuments.Where(doc => doc.Name == name).ToArray();

        if (docs.Length == 0)
        {
            Console.WriteLine("Document not found: " + name);
            return;
        }

        foreach (Document doc in docs)
        {
            if (doc is IEncryptable)
            {
                (doc as IEncryptable).Encrypt();

                Console.WriteLine("Document encrypted: " + name);
            }
            else
            {
                Console.WriteLine("Document does not support encryption: " + name);
            }
        }
    }

    private static void DecryptDocument(string name)
    {
        Document[] docs = allDocuments.Where(doc => doc.Name == name).ToArray();

        if (docs.Length == 0)
        {
            Console.WriteLine("Document not found: " + name);
            return;
        }

        foreach (Document doc in docs)
        {
            if (doc is IEncryptable)
            {
                (doc as IEncryptable).Decrypt();

                Console.WriteLine("Document decrypted: " + name);
            }
            else
            {
                Console.WriteLine("Document does not support decryption: " + name);
            }
        }
    }

    private static void EncryptAllDocuments()
    {
        var encryptables = allDocuments.Where(doc => doc is IEncryptable).ToArray();
        
        if (encryptables.Length == 0)
        {
            Console.WriteLine("No encryptable documents found");
            return;
        }

        Array.ForEach(encryptables, doc => (doc as IEncryptable).Encrypt());

        Console.WriteLine("All documents encrypted");
    }

    private static void ChangeContent(string name, string content)
    {
        Document[] docs = allDocuments.Where(doc => doc.Name == name).ToArray();

        if (docs.Length == 0)
        {
            Console.WriteLine("Document not found: " + name);
            return;
        }

        foreach (Document doc in docs)
        {
            if (doc is IEditable)
            {
                (doc as IEditable).ChangeContent(content);

                Console.WriteLine("Document content changed: " + name);
            }
            else
            {
                Console.WriteLine("Document is not editable: " + name);
            }
        }
    }
}
