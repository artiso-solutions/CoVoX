using System;
using System.Text;
using Spectre.Console;

namespace SpeechTranslation
{
    enum OutputFormat
    {
        Csv,
        Table,
        Markdown
    }

    class DataCollector
    {
        public DateTime Start { get; private set; }

        private readonly object _sync = new object();

        private readonly OutputFormat _outputFormat;
        private readonly string _inputLanguage;
        private readonly StringBuilder _buffer;
        private Table _table;

        public DataCollector(string inputLanguage, OutputFormat outputFormat)
        {
            _inputLanguage = inputLanguage;
            _outputFormat = outputFormat;

            _buffer = new StringBuilder();
            _table = new Table();
            InitTable();
        }

        private void ReinitTable()
        {
            _table = new Table();
            InitTable();
        }

        private void InitTable()
        {
            // Add some columns
            _table
                .AddColumn("Lang")
                .AddColumn("Event")
                .AddColumn("Result")
                .AddColumn(new TableColumn("Delay").Alignment(Justify.Right));

            if (_outputFormat == OutputFormat.Markdown)
                _table.Border = TableBorder.Markdown;
        }

        public void RestartTime() => Start = DateTime.Now;

        public void RegisterEvent(string @event, string? result = null)
        {
            lock (_sync)
            {
                var diff = DateTime.Now - Start;
                var msDiff = $"{diff.TotalMilliseconds} ms";

                if (_outputFormat == OutputFormat.Csv)
                    _buffer.AppendLine($"{_inputLanguage};{@event};{result};{msDiff}");
                else
                    _table.AddRow(_inputLanguage, @event, result ?? "", msDiff);
            }
        }

        public void RegisterSeparator()
        {
            const string separator = "----------";

            lock (_sync)
            {
                if (_outputFormat == OutputFormat.Csv)
                    _buffer.AppendLine(new string('-', separator.Length * 5));
                else
                {
                    var render = new Markup($"[teal]{separator}[/]");
                    _table.AddRow(render, render, render, render);
                }
            }
        }

        public void Render()
        {
            lock (_sync)
            {
                if (_outputFormat == OutputFormat.Csv)
                {
                    Console.Write(_buffer.ToString());
                    _buffer.Clear();
                }
                else
                {
                    AnsiConsole.Render(_table);
                    ReinitTable();
                }
            }
        }
    }
}
