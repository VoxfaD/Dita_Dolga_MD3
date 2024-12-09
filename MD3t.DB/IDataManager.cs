using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD3t.DB
{
    public interface IDataManager
    {
        // Metode, kas atgriež informāciju kā drukātu tekstu par visiem kolekcijās esošajiem elementiem
        string Print();

        // Metode, kas saglabā visu kolekciju datus failā
        void Save(string filePath);

        // Metode, kas nolasa visu kolekciju datus no faila
        void Load(string filePath);

        // Metode, kas izveido testa datus
        void CreateTestData();

        // Metode, kas izdzēš visus datus
        void Reset();
    }
}
