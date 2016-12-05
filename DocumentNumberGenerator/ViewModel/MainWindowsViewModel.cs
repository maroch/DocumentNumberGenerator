using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using DocumentNumberGenerator.Operations;
using DocumentNumberGenerator.Exceptions;

namespace DocumentNumberGenerator.ViewModel
{
    /// <summary>
    /// View Model for MainView, MVVM pattern
    /// This is ViemModel that represents MainWindow Logic, 
    /// </summary>
    public class MainWindowsViewModel : ViewModelBase
    {
        private readonly BackgroundWorker _worker;
        private const int MinimumDocumentNumber = 10000000;
        private const int MaximumDocumentNumber = 99000000;
        private const int MaximumDocumentDatabaseSize = 10000000;
        private const int PackToSaveSize = 20000;
        private decimal _currentProgress;
        private string _dataBaseSizeMessage;
        private int _elementsToFill = 1;
        private int _dataBaseStartSize = 0;

        /// <summary>
        /// Start create documents numbers command
        /// </summary>
        public ICommand InstigateWorkCommand { get; }

        /// <summary>
        /// Bounded current process ProgressBar value
        /// </summary>
        public decimal CurrentProgress
        {
            get { return _currentProgress; }
            private set
            {
                if (_currentProgress != value)
                {
                    _currentProgress = value;
                    RaisePropertyChangedEvent("CurrentProgress");

                }
            }
        }

        /// <summary>
        /// Summary of database elements
        /// </summary>
        public string DatabseCountValue
        {
            get { return _dataBaseSizeMessage; }
            set
            {
                _dataBaseSizeMessage = value;
                RaisePropertyChangedEvent("DatabseCountValue");
            }
        }

        /// <summary>
        /// NUmber of document numbers requested to create
        /// </summary>
        public int ElementsToFill
        {
            get { return _elementsToFill; }
            set
            {
                _elementsToFill = value;
                RaisePropertyChangedEvent("ElementsToFill");
            }
        }

        /// <summary>
        /// Constructor, initialize background worker evens and information about database
        /// </summary>
        public MainWindowsViewModel()
        {
            InstigateWorkCommand = new
                RelayCommand(o => _worker.RunWorkerAsync(), o => !_worker.IsBusy);
            _worker = new BackgroundWorker();
            _worker.DoWork += DoFillDataBaseWork;
            _worker.RunWorkerCompleted += EndFillDataBaseWork;
            _worker.ProgressChanged += ProgressChanged;
            _dataBaseStartSize = DataBaseAccessFactory.GetDataBaseOperationClass().GetDataBaseCount();
            DatabseCountValue = $"{_dataBaseStartSize}\t {Math.Round((double)_dataBaseStartSize / (MaximumDocumentDatabaseSize/100), 2)}%";
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void DoFillDataBaseWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var timeElapsed = FillDataTableRecursive(MinimumDocumentNumber, MaximumDocumentNumber,
                    _elementsToFill);
                MessageBox.Show($"Work Time: {timeElapsed.ToString()}msec", caption: "Operation completed !!!",
                    button: MessageBoxButton.OK, icon: MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, caption: "Operation Error !!!",
                  button: MessageBoxButton.OK, icon: MessageBoxImage.Error);

            }
        }

        /// <summary>
        /// Occurs when worker is end. This event refresh main window controls statuses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndFillDataBaseWork(object sender, RunWorkerCompletedEventArgs e)
        {
            CurrentProgress = 0;
            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(CommandManager.InvalidateRequerySuggested));
        }

        /// <summary>
        /// Main funcion, draws document number from specified range, save them to local database
        /// </summary>
        /// <param name="minimumNumber"></param>
        /// <param name="biggestNumber"></param>
        /// <param name="fillSize"></param>
        /// <returns>Time in milisec that task endure</returns>
        public long FillDataTableRecursive(int minimumNumber = MaximumDocumentNumber,
            int biggestNumber = MaximumDocumentNumber, int fillSize = PackToSaveSize)
        {
            try
            {
                if (fillSize >= MaximumDocumentDatabaseSize || fillSize <= 0)
                    throw new DocumentNumberGeneratorLogicException($"Range of documents can be from 0 to {MaximumDocumentNumber - MinimumDocumentNumber}");
                var watch = Stopwatch.StartNew();
                var maxNumbers = fillSize;
                var numbers = new List<int>(maxNumbers);
                var r = new Random();
                //count of document in database in processed range
                var allStuff = DataBaseAccessFactory.GetDataBaseOperationClass()
                    .GetDataBaseCount(minimumNumber, biggestNumber);
                //number of documents in first half of processed range
                var firstPartStuff = DataBaseAccessFactory.GetDataBaseOperationClass()
                    .GetDataBaseCount(minimumNumber, ((biggestNumber + minimumNumber) / 2));
                var secondPartStuff = allStuff-firstPartStuff;
                if (fillSize > MaximumDocumentDatabaseSize - allStuff)
                    throw new DocumentNumberGeneratorLogicException($"Number of documents can be saved is {MaximumDocumentDatabaseSize - allStuff}");


                //Linear random variable give best range to serch random unige numbers, it seems to be 
                //minimum when full range recurse divided by 2 give minnimum  
                if ((((biggestNumber - minimumNumber) - allStuff) > 2 * fillSize))
                {
                    if (firstPartStuff > secondPartStuff )
                    {
                        return FillDataTableRecursive((biggestNumber + minimumNumber) / 2, biggestNumber,
                            fillSize);
                    }
                    else
                    {
                        return FillDataTableRecursive(minimumNumber, ((biggestNumber + minimumNumber) / 2),
                            fillSize);
                    }
                }


                //Start MAIN STUFF...
                //
                while (maxNumbers > 0)
                {
                    //We can change logic in this section, random pack of documents number trying to 
                    //save in database by "INSERT OR IGNORE" statment, then we check how many document number wasnt saved
                    //and we proccess all logic in full range with number ot not inserted document number 
                    //to change this, need to uncomment and we guarante to there is no document number in database
                    //and need to comment/remove code lines when we check how many document number didnt save  
                    var testValue = r.Next(minimumNumber, biggestNumber);
                    if ((!numbers.Exists(number => number == testValue)))
                        //  && !DataBaseAccessFactory.GetDataBaseOperationClass()
                        // .GetDataDaseExistElement(testValue))
                        numbers.Add(testValue);

                    if ((numbers.Count >= PackToSaveSize) || (maxNumbers == numbers.Count))
                    {
                        var countInsertedValues =
                            DataBaseAccessFactory.GetDataBaseOperationClass().FillDataTable(numbers);
                        _dataBaseStartSize += countInsertedValues;
                        maxNumbers -= countInsertedValues;
                        CurrentProgress += (decimal)(100 * (countInsertedValues))/_elementsToFill;
                        DatabseCountValue = $"{_dataBaseStartSize}\t{Math.Round((double)(_dataBaseStartSize) / ((MaximumDocumentDatabaseSize) / 100), 2)}%";

                        numbers.Clear();
                        if (maxNumbers > 0)
                        {
                            FillDataTableRecursive(MinimumDocumentNumber, MaximumDocumentNumber, maxNumbers);
                            break;
                        }
                    }
                }
                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
            catch (DocumentNumberGeneratorLogicException ex)
            {
                throw new Exception($@"Some Exception error: {ex.Message}.");

            }
            catch (Exception ex)
            {
                throw new Exception($@"Some Exception error: {ex.Message}.");

            }
        }
    }
}