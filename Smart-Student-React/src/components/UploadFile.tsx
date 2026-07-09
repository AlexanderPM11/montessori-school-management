import React, { useState } from "react";
import {
  IoClose,
  IoCheckmarkDone,
  IoCloudUploadOutline,
  IoAlertCircle,
} from "react-icons/io5";

interface Props {
  title?: string;
  acceptFiles?: string;
  file: File | null;
  onClose: () => void;
  onUpload: (
    file: File,
    onProgress: (progress: number, error?: string) => void
  ) => Promise<void>;
  onSuccessClick?: () => void;
  onFileSelected?: (file: File) => void; // ✅ NUEVA PROP
}

export const UploadFile: React.FC<Props> = ({
  title,
  acceptFiles,
  file,
  onClose,
  onUpload,
  onSuccessClick,
  onFileSelected, // ✅ DESTRUCTURACIÓN
}) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(file);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [progress, setProgress] = useState<number>(0);
  const [statusMessage, setStatusMessage] = useState<string>("");
  const [uploadComplete, setUploadComplete] = useState<boolean>(false);
  const [error, setError] = useState<string>("");

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setSelectedFile(file);
      resetStatus();
      onFileSelected?.(file); // ✅ NOTIFICAR ARCHIVO
    }
  };

  const resetStatus = () => {
    setUploadComplete(false);
    setProgress(0);
    setError("");
    setStatusMessage("");
  };

  const handleUpload = async () => {
    if (!selectedFile) return;

    resetStatus();
    setIsLoading(true);
    setStatusMessage("Cargando...");

    try {
      await onUpload(selectedFile, (currentProgress, errorMessage) => {
        setProgress(currentProgress);
        if (errorMessage) {
          setError(errorMessage);
          setIsLoading(false);
        } else if (currentProgress === 100) {
          setStatusMessage("Completado");
          setUploadComplete(true);
          setIsLoading(false);
        }
      });
    } catch (error) {
      console.error("Error al subir el archivo:", error);
      setError(error instanceof Error ? error.message : "Error desconocido");
      setIsLoading(false);
    }
  };

  const handleSuccessClick = () => {
    if (uploadComplete && onSuccessClick) {
      onSuccessClick();
    }
  };

  return (
    <div className="fixed inset-0 min-h-screen overflow-y-auto bg-black bg-opacity-5 flex items-center justify-center z-50">
      <div className="bg-white rounded-xl shadow-lg p-6 w-full max-w-md relative">
        <button
          onClick={onClose}
          className="absolute top-2 right-2 text-gray-500 hover:text-gray-800"
        >
          <IoClose size={20} />
        </button>

        <h2 className="text-lg font-semibold mb-4">
          {title ?? "Cargar archivo"}
        </h2>

        {selectedFile ? (
          <div className="border border-dashed border-gray-400 rounded-md p-4 text-center mb-4">
            <p className="text-sm text-gray-600">Archivo seleccionado:</p>
            <p className="font-medium">{selectedFile.name}</p>
          </div>
        ) : (
          <p className="text-sm text-gray-500 italic mb-4">
            No se ha seleccionado ningún archivo
          </p>
        )}

        <div className="flex justify-start mb-4">
          <input
            type="file"
            accept={acceptFiles ?? ".xlsx, .xls"}
            className="hidden"
            id="file-upload"
            onChange={handleFileChange}
          />
          <label
            htmlFor="file-upload"
            className={`border ${
              uploadComplete
                ? "border-green-500"
                : error
                ? "border-red-500"
                : "border-gray-900"
            } text-gray-900 px-4 h-10 flex items-center rounded-full cursor-pointer hover:bg-gray-900 hover:text-white transition-colors text-sm`}
          >
            Seleccionar archivo
            <IoCloudUploadOutline className="ml-1" />
          </label>
        </div>

        {(isLoading || uploadComplete || error) && (
          <div className="w-full mt-4">
            <div className="w-full bg-gray-300 rounded-full h-2 overflow-hidden">
              <div
                className={`h-2 rounded-full transition-all duration-300 ease-out ${
                  uploadComplete
                    ? "bg-green-500"
                    : error
                    ? "bg-red-500"
                    : "bg-gray-900"
                }`}
                style={{ width: `${progress}%` }}
              />
            </div>

            <div className="flex items-center justify-center gap-2 mt-2">
              {uploadComplete ? (
                <IoCheckmarkDone className="text-green-500" size={18} />
              ) : error ? (
                <IoAlertCircle className="text-red-500" size={18} />
              ) : null}
              <p
                className={`text-center text-sm ${
                  uploadComplete
                    ? "text-green-500"
                    : error
                    ? "text-red-500"
                    : "text-gray-600"
                }`}
              >
                {error || statusMessage}{" "}
                {!error && !uploadComplete && `${Math.round(progress)}%`}
              </p>
            </div>
          </div>
        )}

        <div className="flex justify-end gap-2 mt-6">
          <button
            onClick={onClose}
            className="border border-gray-400 text-gray-700 rounded-full px-4 py-1.5 text-sm hover:bg-gray-100 transition"
          >
            Cancelar
          </button>
          <button
            onClick={uploadComplete ? handleSuccessClick : handleUpload}
            disabled={isLoading || !selectedFile}
            className={`flex items-center gap-2 rounded-full px-4 py-1.5 text-sm text-white transition ${
              isLoading || !selectedFile
                ? uploadComplete
                  ? "bg-green-500 cursor-pointer"
                  : error
                  ? "bg-red-500 cursor-not-allowed"
                  : "bg-gray-400 cursor-not-allowed"
                : uploadComplete
                ? "bg-green-500 hover:bg-green-600"
                : "bg-gray-900 hover:bg-gray-700"
            }`}
          >
            {error ? (
              <IoAlertCircle size={18} />
            ) : uploadComplete ? (
              <IoCheckmarkDone size={18} />
            ) : (
              <IoCloudUploadOutline />
            )}
            {error
              ? "Reintentar"
              : uploadComplete
              ? "¡Listo!"
              : "Comenzar carga"}{" "}
          </button>
        </div>
      </div>
    </div>
  );
};
