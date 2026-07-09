using MontessoriSystem.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;

namespace MontessoriSystem.Core.Application.Helpers
{
    public class FileServices: IFileServices
    {
        private readonly GeneralResponse<string> response = new GeneralResponse<string> { result = true };

        public async Task<GeneralResponse<string>> CreateOrUpdateFile(IFormFile Archivo,string pathFile)
        {   
            return await CreateFile(Archivo, pathFile);            
        }
        public async Task DeleteFile(string path)
        {
            try
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                var filePath = Path.Combine(uploadsPath, path);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el archivo: {ex.Message}");
            }
        }
        public async Task<GeneralResponse<string>> ConvertFileToBase64(IFormFile archivo)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await archivo.CopyToAsync(memoryStream);

                    byte[] fileBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(fileBytes);

                    // Obtener el tipo MIME del archivo
                    string mimeType = GetMimeType(archivo.FileName);

                    // Prerender el tipo MIME a la cadena Base64
                    string base64WithMime = $"data:{mimeType};base64,{base64String}";

                    response.Data = base64WithMime;
                    response.result = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                var error = ($"Error al convertir el archivo: {ex.Message}");

                response.Data = null;
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }
        public async Task<GeneralResponse<string>> GetImageAsBase64(string pathFile)
        {
            try
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                var filePath = Path.Combine(uploadsPath, pathFile);

                string mimeType = GetMimeType(pathFile);

                var imageBytes = System.IO.File.ReadAllBytes(filePath);

                string base64String = Convert.ToBase64String(imageBytes);

                // Prerender el tipo MIME a la cadena Base64
                string base64WithMime = $"data:{mimeType};base64,{base64String}";

                response.Data = base64WithMime;
                response.result = true;

                return response;
            }
            catch (Exception ex)
            {
                var error = ($"Error al convertir el archivo: {ex.Message}");
                response.result = false;
                response.messages.Add(error);

                return response;
            }
            
        }
        private async Task<GeneralResponse<string>> CreateFile(IFormFile archivo, string pathNewFolder)
        {            
            try
            {
                var nameFile = archivo.FileName.Substring(0, Math.Min(archivo.FileName.Length, 10));

                var nombreArchivo = $"{Guid.NewGuid().ToString()}_{nameFile}{Path.GetExtension(archivo.FileName)}";

                // Construir la ruta completa del archivo de destino
                var rutaDestino = Path.Combine(pathNewFolder, nombreArchivo);

                // Copiar el archivo al destino
                using (var stream = new FileStream(rutaDestino, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                response.Data = nombreArchivo;
                response.messages.Add("Archivo creado exitosamente");
                return response;
            }
            catch (Exception ex)
            {
                response.messages.Add("Ocurrio un error al intentar crear el archvio" + ex.Message);
                response.result=false;
                return response;
            }
        }
        private async Task UpdateFileContent(string path, string content)
        {
            try
            {
                await File.WriteAllTextAsync(path, content);
                Console.WriteLine($"Archivo actualizado en {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el archivo: {ex.Message}");
            }
        }
        private string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                ".mp3" => "audio/mpeg",
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".json" => "application/json",
                ".xml" => "application/xml",
                _ => "application/octet-stream", // Si el tipo no se reconoce
            };
        }

    }
}