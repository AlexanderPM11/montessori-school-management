import * as Dialog from "@radix-ui/react-dialog";
import { FiArrowLeft, FiDownload } from "react-icons/fi";

export const DocumentViewerDialog = ({
  isOpen,
  onClose,
  document,
  onDownload,
}: {
  isOpen: boolean;
  onClose: () => void;
  document: { name: string; base64: string };
  onDownload: () => void;
}) => {
  if (!document?.base64) return null;

  return (
    <Dialog.Root open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <Dialog.Portal>
        <Dialog.Overlay className="fixed inset-0 bg-black/50 z-50 " />
        <Dialog.Content className="fixed top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 z-50 flex flex-col bg-white rounded-lg shadow-xl overflow-hidden max-h-[90vh] w-full max-w-[450px] md:max-w-[800px]">
          {/* Header */}
          <div className="sticky top-0 z-10 bg-white flex justify-between items-center border-b p-4 ">
            <Dialog.Title className="text-lg font-semibold">
              {document.name}
            </Dialog.Title>
            <Dialog.Close asChild>
              <button
                className="text-gray-500 hover:text-gray-700 text-2xl"
                aria-label="Cerrar"
              >
                &times;
              </button>
            </Dialog.Close>
          </div>

          {/* Content */}
          <div className="flex-1 p-4 overflow-auto ">
            {document.base64.includes("image") ? (
              <div className="max-h-[65vh] overflow-auto">
                <img
                  src={document.base64}
                  alt={document.name}
                  className="max-w-full h-auto mx-auto"
                />
              </div>
            ) : document.base64.includes("pdf") ? (
              <iframe
                src={document.base64}
                className="w-full h-[65vh]"
                title="Documento PDF"
              ></iframe>
            ) : (
              <div className="text-center py-10">
                <p>Formato no soportado para vista previa.</p>
                <a
                  href={document.base64}
                  download={document.name}
                  className="text-blue-500 underline mt-2 inline-block"
                >
                  Descargar para ver el contenido
                </a>
              </div>
            )}
          </div>

          {/* Footer */}
          <div className="border-t p-4 flex justify-end">
            <button
              type="button"
              onClick={onClose}
              className="w-32 px-4 py-2 bg-gray-300 text-gray-800 rounded-lg hover:bg-gray-400 transition-colors mr-4 flex items-center justify-center gap-2"
            >
              <FiArrowLeft className="text-lg" />
              Regresar
            </button>

            <button
              onClick={onDownload}
              className="bg-gray-900 text-white px-4 py-2 rounded hover:bg-gray-800 flex items-center"
            >
              <FiDownload className="mr-2" />
              Descargar
            </button>
          </div>
        </Dialog.Content>
      </Dialog.Portal>
    </Dialog.Root>
  );
};
