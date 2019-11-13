using System;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;

// https://docs.unity3d.com/ScriptReference/Windows.Speech.KeywordRecognizer.html

public class kwRecognizer : MonoBehaviour
{
    [SerializeField]
    private string[] m_Keywords = new string[] {
        "rook", "bishop", "king", "pawn", "knight", "queen",
        "a","b","c","d","e","f","g","h",
        "1","2","3","4","5","6","7","8"
    };

    private KeywordRecognizer m_Recognizer;
    public Queue<MovementInfo> Buffer = new Queue<MovementInfo>();

    public class MovementInfo
    {
        public string name;

        public int fromRow;
        public int fromCol;

        public int toRow;
        public int toCol;

        public MovementInfo(string [] fields){
            name = fields[0].ToLower();

            Int32.TryParse(fields[1], out fromRow);
            Int32.TryParse(fields[2], out fromCol);
            Int32.TryParse(fields[3], out toRow);
            Int32.TryParse(fields[4], out toCol);

            fromCol--;
            fromRow--;
            toCol--;
            toRow--;
        }
    }

    private GUIText  gt;
    void Start()
    {
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();

        gt = GetComponent<GUIText>();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        // Buffer.Enqueue(args.text);
    }


    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (gt.text.Length != 0)
                {
                    gt.text = gt.text.Substring(0, gt.text.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                // print("User entered their name: " + gt.text);
                var fields = gt.text.Split(' ');
                gt.text = "";
                var piece = new MovementInfo(fields);
                Buffer.Enqueue(piece);
                Debug.Log("SENT!");
            }
            else
            {
                gt.text += c;
            }
            Debug.Log(gt.text);
        }
    }
}