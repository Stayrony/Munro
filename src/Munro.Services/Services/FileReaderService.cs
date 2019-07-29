using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Munro.Common.Invoke;
using Munro.Services.Contract.Services;
using Munro.Models.Models;

namespace Munro.Services.Services
{
    public class FileReaderService : IFileReaderService
    {
        private readonly IInvokeHandler<FileReaderService> _invokeHandler;

        private readonly List<string> _supportedFileTypes = new List<string>
        {
            ".csv"
        };

        public FileReaderService(
            IInvokeHandler<FileReaderService> invokeHandler)

        {
            _invokeHandler = invokeHandler;
        }

        public async Task<InvokeResult<IEnumerable<MunroFullModel>>> UploadMunrosFileAsync(IFormFile file)
            => await _invokeHandler.InvokeAsync(async () =>
            {
                if (file == null)
                {
                    return InvokeResult<IEnumerable<MunroFullModel>>.Fail(ResultCode.ValidationError,
                        "You need to attach a .csv file");
                }

                string extension = Path.HasExtension(file.FileName)
                    ? Path.GetExtension(file.FileName)
                    : string.Empty;

                var isSupported = IsFileTypeSupported(extension);

                if (!isSupported)
                {
                    var errorMessage = "File Extension Is Unsupported";
                    return InvokeResult<IEnumerable<MunroFullModel>>.Fail(ResultCode.UnsupportedFileExtension,
                        errorMessage);
                }

                var munroFullModels = new List<MunroFullModel>();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var header = true;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        if (header)
                        {
                            header = false;
                            continue;
                        }

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        var values = line.Split(',');
                        if (values.Count() != 30)
                        {
                            continue;
                        }

                        munroFullModels.Add(new MunroFullModel
                        {
                            RunningNo = values[0],
                            DoBIHNumber = values[1],
                            Streetmap = $"{values[2]},{values[3]}",
                            Geograph = values[4],
                            HillBagging = values[5],
                            Name = values[6],
                            SMCSection = values[7],
                            RHBSection = values[8],
                            Section = values[9],
                            Heightm = values[10],
                            Heightft = values[11],
                            Map150 = values[12],
                            Map125 = values[13],
                            GridRef = values[14],
                            GridRefXY = values[15],
                            Xcoord = values[16],
                            Ycoord = values[17],
                            HillCategory1891 = values[18],
                            HillCategory1921 = values[19],
                            HillCategory1933 = values[20],
                            HillCategory1953 = values[21],
                            HillCategory1969 = values[22],
                            HillCategory1974 = values[23],
                            HillCategory1981 = values[24],
                            HillCategory1984 = values[25],
                            HillCategory1990 = values[26],
                            HillCategory1997 = values[27],
                            HillCategoryPost1997 = values[28],
                            Comments = values[29]
                        });
                    }
                }

                return InvokeResult<IEnumerable<MunroFullModel>>.Ok(munroFullModels);
            });

        #region -- Private methods --

        private bool IsFileTypeSupported(string src)
        {
            string extension = Path.HasExtension(src)
                ? Path.GetExtension(src)?.ToLower()
                : string.Empty;

            if (_supportedFileTypes.Contains(extension))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}