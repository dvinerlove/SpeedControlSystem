using ClassLibrary;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Diagnostics;
using System.Text;

namespace SpeedControlSystemWeb.Models
{
    public static class DataSearcher
    {
        public static string FolderPath { get; set; }
        public static string Filename { get; set; }
        static FSDirectory? fSDirectory;
        static Lucene.Net.Util.Version appLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

        public static bool AddItem(SpeedReport speedReport)
        {
            using (var analyzer = new StandardAnalyzer(appLuceneVersion))
            using (var writer = new IndexWriter(fSDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {

                var result = Search(hash: speedReport.Hash);
                if (result.Count == 0)
                {
                    using (StreamWriter stream = new FileInfo(FolderPath+Filename).AppendText())
                    {
                        stream.WriteLine($"{speedReport.Hash},{speedReport.DateTime},{speedReport.Number},{speedReport.Speed.ToString().Replace(",", ".")}");
                    }
                    Document document = GetDocument(speedReport);
                    writer.AddDocument(document);
                    writer.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static Document GetDocument(SpeedReport speedReport)
        {
            Document document = new Document();

            document.Add(new Field("hash", speedReport.Hash, Field.Store.NO, Field.Index.ANALYZED));
            document.Add(new Field("hash_exact", speedReport.Hash, Field.Store.YES, Field.Index.NOT_ANALYZED));

            var dateField = new NumericField("date", Field.Store.YES, true);
            dateField.OmitNorms = false;

            dateField.OmitTermFreqAndPositions = false;
            dateField.SetLongValue(speedReport.DateTime);
            document.Add(dateField);

            document.Add(new Field("number", speedReport.Number, Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));

            var speedField = new NumericField("speed", Field.Store.YES, true);
            speedField.OmitNorms = false;

            speedField.OmitTermFreqAndPositions = false;
            speedField.SetDoubleValue(speedReport.Speed);
            document.Add(speedField);
            return document;
        }

        public static void IndexFile()
        {

            var indexLocation = FolderPath + @"Index\";

            if (System.IO.Directory.Exists(indexLocation) == false)
            {
                System.IO.Directory.CreateDirectory(indexLocation);
            }

            var filePath = FolderPath + Filename;

            fSDirectory = FSDirectory.Open(indexLocation);

            var splitChar = new char[] { ',' };

            if (IndexReader.IndexExists(fSDirectory))
            {
                return;
            }

            using (var analyzer = new StandardAnalyzer(appLuceneVersion))
            using (var writer = new IndexWriter(fSDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                const Int32 BufferSize = 128;
                using (var fileStream1 = File.OpenRead(filePath))
                using (var streamReader = new StreamReader(fileStream1, Encoding.UTF8, true, BufferSize))
                {
                    String? line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var values = line.Split(splitChar);

                        Document document = GetDocument(new SpeedReport() { DateTime = long.Parse(values[1]), Number = values[2], Speed = double.Parse(values[3].Replace(".", ",")) });
                        writer.AddDocument(document);
                    }
                    writer.Commit();
                }
                return;
            }
        }

        public static List<string> Search(double? speed = null, double? speedMax = null, object? date = null, object? dateMax = null, string? hash = null)
        {

            var analyzer = new StandardAnalyzer(appLuceneVersion);
                var indexLocation = FolderPath + @"Index\";
            var dir = FSDirectory.Open(indexLocation);

            IndexSearcher searcher = new IndexSearcher(dir);

            var query = new BooleanQuery();

            DateTime dateFirst;
            DateTime dateSecond;
            if (date != null && dateMax == null)
            {
                dateFirst = DateTime.Parse(date.ToString()!).Date;
                dateSecond = DateTime.Parse(date.ToString()!).AddDays(1).Date;
                var nrq = NumericRangeQuery.NewLongRange("date", dateFirst.Ticks, dateSecond.Ticks, true, true);
                query.Add(nrq, Occur.MUST);
            }

            if (date != null && dateMax != null)
            {
                dateFirst = DateTime.Parse(date.ToString()!).Date;
                dateSecond = DateTime.Parse(dateMax.ToString()!).AddDays(1).Date;
                var nrq = NumericRangeQuery.NewLongRange("date", dateFirst.Ticks, dateSecond.Ticks, true, true);
                query.Add(nrq, Occur.MUST);
            }

            if (speed != null && speed > 0)
            {
                var nrq = NumericRangeQuery.NewDoubleRange("speed", speed, null, true, true);
                query.Add(nrq, Occur.MUST);
            }

            if (hash != null && string.IsNullOrEmpty(hash) == false)
            {
                Term searchTerm = new Term("hash", hash.ToLowerInvariant());
                query.Add(new BooleanClause(new TermQuery(searchTerm), Occur.MUST));
            }

            var hits = searcher.Search(query, 10000);

            var results = new List<string>();

            foreach (var hit in hits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);
                var report = new SpeedReport
                {
                    DateTime = long.Parse(doc.Get("date")),
                    Number = doc.Get("number"),
                    Speed = double.Parse(doc.Get("speed").Replace(".", ","))
                };
                results.Add(report.ToString());
            }

            return results;
        }
    }
}
