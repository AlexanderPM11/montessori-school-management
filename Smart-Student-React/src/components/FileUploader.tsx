import { useState } from "react";

interface FileUploaderProps {
  onFileSelect: (
    fileLoad: File | null,
    base64: string,
    type: "image" | "pdf" | "unknown"
  ) => void;
  initialBase64?: string;
  title?: string;
  accept?: string;
}

export const FileUploader = ({
  onFileSelect,
  initialBase64,
  title,
  accept,
}: FileUploaderProps) => {
  const [base64, setBase64] = useState<string>(initialBase64 || "");
  const [isDragging, setIsDragging] = useState(false);

  const fileTypeInit = (base64: string): "image" | "pdf" | "unknown" => {
    if (base64 === "" || base64 === undefined) return "unknown";
    if (base64.includes("image")) return "image";
    else if (base64.includes("pdf")) return "pdf";
    else return "unknown";
  };
  const [fileType, setFileType] = useState<"image" | "pdf" | "unknown">(
    fileTypeInit(base64)
  );

  const handleFile = (file: File) => {
    const reader = new FileReader();
    reader.onload = () => {
      const result = reader.result as string;
      setBase64(result);

      if (file.type.includes("image")) setFileType("image");
      else if (file.type.includes("pdf")) setFileType("pdf");
      else setFileType("unknown");

      onFileSelect(
        file,
        result,
        file.type.includes("image")
          ? "image"
          : file.type.includes("pdf")
          ? "pdf"
          : "unknown"
      );
    };
    reader.readAsDataURL(file);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) handleFile(file);
  };

  const handleDragOver = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    setIsDragging(true);
  };

  const handleDragLeave = () => setIsDragging(false);

  const handleDrop = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    setIsDragging(false);
    const file = e.dataTransfer.files?.[0];
    if (file) handleFile(file);
  };

  const clearFile = () => {
    setBase64("");
    setFileType("unknown");
    onFileSelect(null, "", "unknown");
  };

  return (
    <div className="mb-6">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {title ?? "Archivo *"}
      </label>

      <div
        className={`mt-1 border-2 border-dashed rounded-lg p-6 text-center transition-colors ${
          isDragging
            ? "border-blue-500 bg-blue-50"
            : "border-gray-300 hover:border-gray-400"
        }`}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
        onDrop={handleDrop}
      >
        {!base64 ? (
          <div className="flex flex-col items-center justify-center space-y-2">
            <svg
              className="h-10 w-10 text-gray-400"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12"
              />
            </svg>
            <p className="text-sm text-gray-600">
              {isDragging
                ? "Suelta tu archivo aquí"
                : "Arrastra y suelta tu archivo aquí"}
            </p>
            <p className="text-xs text-gray-500">
              Formatos soportados: imágenes y PDF
            </p>
            <label className="cursor-pointer mt-2 bg-white py-2 px-3 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50">
              Seleccionar archivo
              <input
                type="file"
                accept={accept ?? "image/*,application/pdf"}
                onChange={handleFileChange}
                className="hidden"
              />
            </label>
          </div>
        ) : (
          <div className="flex flex-col items-center">
            {fileType === "image" ? (
              <div className="max-h-96 overflow-hidden mb-2">
                <img
                  src={base64}
                  alt="Previsualización"
                  className="max-w-full h-auto rounded"
                />
              </div>
            ) : fileType === "pdf" ? (
              <div className="flex flex-col items-center p-2 mb-2">
                <svg
                  className="h-16 w-16 text-red-500"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z"
                  />
                </svg>
                <span className="mt-1 text-sm font-medium text-gray-700">
                  Documento PDF
                </span>
              </div>
            ) : (
              <div className="p-2 text-center mb-2">
                <span className="text-sm text-gray-500">
                  Archivo cargado (tipo no soportado para previsualización)
                </span>
              </div>
            )}
            <button
              type="button"
              onClick={clearFile}
              className="mt-2 text-sm text-red-600 hover:text-red-800"
            >
              Eliminar archivo
            </button>
          </div>
        )}
      </div>
    </div>
  );
};
