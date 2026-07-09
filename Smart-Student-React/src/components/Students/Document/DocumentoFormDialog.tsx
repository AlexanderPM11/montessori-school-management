import * as Dialog from "@radix-ui/react-dialog";
import { FiX } from "react-icons/fi";
import { Adjunto } from "../../../interfaces/Adjunto/Ajunto";
import { CreateOrEditForm } from "../../../Formik/Adjunto/CreateOrEdit";

interface DocumentoFormDialogProps {
  isOpen: boolean;
  onClose: () => void;
  initialData?: Adjunto;
}

export const DocumentoFormDialog: React.FC<DocumentoFormDialogProps> = ({
  isOpen,
  onClose,
  initialData,
}) => {
  return (
    <Dialog.Root open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <Dialog.Portal>
        <Dialog.Overlay className="fixed inset-0 bg-black/50 z-50" />
        <Dialog.Content className="fixed top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 z-50 flex flex-col bg-white rounded-lg shadow-xl overflow-hidden max-h-[95vh] w-full max-w-[600px] md:max-w-[800px]">
          {/* Header */}
          <div className="sticky top-0 z-10 bg-white flex justify-between items-center border-b p-4">
            <Dialog.Title className="text-lg font-semibold text-gray-800">
              {initialData ? "Editar Documento" : "Nuevo Documento"}
            </Dialog.Title>
            <Dialog.Close asChild>
              <button
                className="text-gray-500 hover:text-gray-700 p-2"
                aria-label="Cerrar"
              >
                <FiX size={24} />
              </button>
            </Dialog.Close>
          </div>

          {/* Formulario */}
          <div className="p-4 overflow-auto flex-1">
            <CreateOrEditForm onClose={onClose} />
          </div>
        </Dialog.Content>
      </Dialog.Portal>
    </Dialog.Root>
  );
};
