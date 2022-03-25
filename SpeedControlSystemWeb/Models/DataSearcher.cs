using ClassLibrary;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace SpeedControlSystemWeb.Models
{
    public static class DataSearcher
    {
       public static void IndexFile(string filename)
        {
            var AppLuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

            var indexLocation = new DirectoryInfo(@"D:/Index/").FullName;
            var dir = FSDirectory.Open(indexLocation);

            var fileStream = new StreamReader(filename);
            string? line = fileStream.ReadLine();


            var splitChar = new char[] { ',' };

            using (var analyzer = new StandardAnalyzer(AppLuceneVersion))
            using (var writer = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                writer.DeleteAll();
                while ((line = fileStream.ReadLine()) != null)
                {
                    var values = line.Split(splitChar);

                    Document document = new Document();

                    document.Add(new Field("id", values[0], Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.NO));

                    var dateField = new NumericField("date", Field.Store.YES, true);
                    dateField.OmitNorms = false;

                    dateField.OmitTermFreqAndPositions = false;
                    dateField.SetLongValue(long.Parse(values[1]));
                    document.Add(dateField);

                    document.Add(new Field("number", values[2], Field.Store.YES, Field.Index.ANALYZED));

                    var speedField = new NumericField("speed", Field.Store.YES, true);
                    speedField.OmitNorms = false;

                    speedField.OmitTermFreqAndPositions = false;
                    speedField.SetDoubleValue(double.Parse(values[3].Replace(".", ",")));
                    document.Add(speedField);

                    writer.AddDocument(document);
                }
            }
        }

       public static List<SpeedReport> Search(int? speed, object? date, object? maxDate)
        {
            var indexLocation = @"D:\Index";
            var dir = FSDirectory.Open(indexLocation);

            IndexSearcher searcher = new IndexSearcher(dir);

            var query = new BooleanQuery();

            DateTime dateFirst;
            DateTime dateSecond;
            if (date != null && maxDate == null)
            {
                dateFirst = DateTime.Parse(date.ToString()!).Date;
                dateSecond = DateTime.Parse(date.ToString()!).AddDays(1).Date;
                var nrq = NumericRangeQuery.NewLongRange("date", dateFirst.Ticks, dateSecond.Ticks, true, true);
                query.Add(nrq, Occur.MUST);
            }

            if (date != null && maxDate != null)
            {
                dateFirst = DateTime.Parse(date.ToString()!).Date;
                dateSecond = DateTime.Parse(maxDate.ToString()!).AddDays(1).Date;
                var nrq = NumericRangeQuery.NewLongRange("date", dateFirst.Ticks, dateSecond.Ticks, true, true);
                query.Add(nrq, Occur.MUST);
            }

            if (speed != null && speed > 0)
            {
                var nrq = NumericRangeQuery.NewDoubleRange("speed", speed, null, true, true);
                query.Add(nrq, Occur.MUST);
            }

            var hits = searcher.Search(query, 10000);

            var results = new List<SpeedReport>();

            foreach (var hit in hits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);

                results.Add(new SpeedReport
                {
                    Id = int.Parse(doc.Get("id")),
                    DateTime = long.Parse(doc.Get("date")),
                    Number = doc.Get("number"),
                    Speed = double.Parse(doc.Get("speed").Replace(".", ","))
                });
            }

            return results;
        }
    }
}
