using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {        
        private StreamReader _localStream;
        private bool _disposed = false;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = false;
            _localStream = new StreamReader(fileFullPath);
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof { get; private set; }
        
        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            if (IsEof)
            {
                throw new IOException("Достигнут конец файла");
            }

            char result = (char)_localStream.Read();

            if (_localStream.Peek() < 0)
            {
                IsEof = true;
            }

            return result;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            _localStream.DiscardBufferedData();
            _localStream.BaseStream.Seek(0, SeekOrigin.Begin);

            IsEof = false;
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            Dispose(true);            
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                _localStream?.Close();
            }                
            
            _disposed = true;            
        }

        /// <summary>
        /// Метод финализации.
        /// </summary>
        ~ReadOnlyStream()
        {
            Dispose(false);
        }
    }
}
