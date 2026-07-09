import { CiEdit } from "react-icons/ci";
import { FiDownload, FiEye, FiFile, FiImage, FiTrash2 } from "react-icons/fi";
import { Adjunto } from "../../../interfaces/Adjunto/Ajunto";

interface CardProps {
  titulo: string;
  descripcion: string;
  base64: string;
  doc: Adjunto;

  // functions
  downloadDocument: (doc: Adjunto) => void;
  openForm: (doc: Adjunto) => void;
  handleDeleteDocument: (doc: Adjunto) => void;
  openDocumentViewer: (doc: Adjunto) => void;
}

export const Card = ({
  titulo,
  descripcion,
  base64,
  doc,
  downloadDocument,
  openForm,
  handleDeleteDocument,
  openDocumentViewer,
}: CardProps) => {
  const getFileIcon = (base64: string | undefined | null) => {
    if (!base64) return <FiFile className="text-gray-400" size={20} />;
    if (base64.includes("image"))
      return <FiImage className="text-blue-500" size={20} />;
    if (base64.includes("pdf"))
      return <FiFile className="text-red-500" size={20} />;
    return <FiFile className="text-gray-400" size={20} />;
  };

  return (
    <div className="border border-gray-200 rounded-xl overflow-hidden shadow-sm hover:shadow-md transition-all duration-200 bg-white flex flex-col h-full w-full max-w-sm">
      {/* Header con título e ícono */}
      <div className="p-4 border-b border-gray-100 flex items-start justify-between">
        <div className="flex items-center gap-3 flex-1">
          <div className="flex-shrink-0 mt-1">{getFileIcon(base64)}</div>
          <div className="min-w-0">
            <h3 className="font-medium text-gray-800 line-clamp-2 max-w-[18rem]">
              {titulo}
            </h3>

            {descripcion && (
              <p className="text-sm text-gray-500 mt-1 line-clamp-6">
                {descripcion}
              </p>
            )}
          </div>
        </div>
      </div>

      {/* Contenido del documento */}
      <div className="flex-1 p-4 h-64 overflow-hidden">
        {base64?.includes("image") ? (
          <div className="relative group max-h-64">
            <div className=" bg-gray-100 rounded-lg overflow-hidden">
              <img
                src={base64}
                alt={titulo}
                className="w-full h-full object-cover cursor-pointer transition-transform duration-300 group-hover:scale-105"
                onClick={() => {
                  openDocumentViewer(doc);
                }}
              />
            </div>
            <div className="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-10 transition-all duration-300 flex items-center justify-center opacity-0 group-hover:opacity-100">
              <button
                onClick={() => {
                  openDocumentViewer(doc);
                }}
                className="bg-white bg-opacity-90 p-2 rounded-full shadow-md hover:bg-opacity-100 transition-all"
              >
                <FiEye className="text-blue-600" size={20} />
              </button>
            </div>
          </div>
        ) : base64?.includes("pdf") ? (
          <div
            className="h-full w-full flex flex-col items-center justify-center bg-gray-50 rounded-lg border border-gray-200 cursor-pointer hover:bg-gray-100 transition-colors"
            onClick={() => openDocumentViewer(doc)}
          >
            <iframe
              src={base64}
              className="w-full h-full rounded-lg"
              title="PDF Preview"
            />
          </div>
        ) : (
          <div className="h-full flex flex-col items-center justify-center bg-gray-50 rounded-lg border border-gray-200 p-6">
            <FiFile className="text-gray-400 mb-3" size={48} />
            <span className="text-sm font-medium text-gray-600">Documento</span>
            <span className="text-xs text-gray-400 mt-1">
              Formato no soportado para vista previa
            </span>
          </div>
        )}
      </div>

      {/* Botones de acción */}
      <div className="border-t border-gray-100 px-4 py-3 bg-gray-50">
        <div className="flex justify-between items-center">
          <div className="flex gap-2">
            <button
              onClick={() => {
                openDocumentViewer(doc);
              }}
              className="p-2 text-gray-500 hover:text-blue-600 rounded-lg hover:bg-blue-50 transition-colors flex items-center gap-1"
              title="Ver"
            >
              <FiEye size={16} />
              <span className="text-xs hidden sm:inline">Ver</span>
            </button>
            <button
              onClick={() => downloadDocument(doc)}
              className="p-2 text-gray-500 hover:text-green-600 rounded-lg hover:bg-green-50 transition-colors flex items-center gap-1"
              title="Descargar"
            >
              <FiDownload size={16} />
              <span className="text-xs hidden sm:inline">Descargar</span>
            </button>
          </div>

          <div className="flex gap-2">
            <button
              onClick={() => openForm(doc)}
              className="p-2 text-gray-500 hover:text-yellow-600 rounded-lg hover:bg-yellow-50 transition-colors flex items-center gap-1"
              title="Editar"
            >
              <CiEdit size={16} />
              <span className="text-xs hidden sm:inline">Editar</span>
            </button>
            <button
              onClick={() => handleDeleteDocument(doc)}
              className="p-2 text-gray-500 hover:text-red-600 rounded-lg hover:bg-red-50 transition-colors flex items-center gap-1"
              title="Eliminar"
            >
              <FiTrash2 size={16} />
              <span className="text-xs hidden sm:inline">Eliminar</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
