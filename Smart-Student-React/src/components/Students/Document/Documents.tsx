import { useEffect, useState } from "react";
import { useAdjuntoStore } from "../../../hooks/store/Adjunto/Adjunto.store";
import { FaPlus } from "react-icons/fa";
import { Adjunto } from "../../../interfaces/Adjunto/Ajunto";
import { useMediaQuery } from "react-responsive";
import ManageLocalStorage from "../../../util/manageLocalStorage";
import { SweetAlertCustom } from "../../../util/SweetAlerCustom";
import { ToastifyCustom } from "../../../util/ToastifyCustom ";
import { Card } from "./Card";
import { useNavigate } from "react-router-dom";
import { DocumentViewerDialog } from "./DocumentViewerDialog";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { DocumentoFormDialog } from "./DocumentoFormDialog";

export const Documents = ({ studentId }: { studentId: number }) => {
  const getAdjunto = useAdjuntoStore((state) => state.getAdjunto);
  const onDelete = useAdjuntoStore((state) => state.onDelete);

  const loading = useAdjuntoStore((state) => state.loading);
  const adjuntos: Adjunto[] = useAdjuntoStore((state) => state.adjuntos);

  const [selectedDoc, setSelectedDoc] = useState<Adjunto | null>(null);
  const [isViewerOpen, setIsViewerOpen] = useState<boolean>(false);
  const [isFormOpen, setIsFormOpen] = useState(false);

  const navigate = useNavigate();

  const isMobile = useMediaQuery({ maxWidth: 768 });

  useEffect(() => {
    getAdjunto(studentId);
  }, [getAdjunto, studentId]);

  const openDocumentViewer = (doc: Adjunto) => {
    setSelectedDoc(doc);
    setIsViewerOpen(true);
  };

  const closeDocumentViewer = () => {
    setIsViewerOpen(false);
    setSelectedDoc(null);
  };

  const downloadDocument = (doc?: Adjunto) => {
    if (!doc?.base64) return;
    const link = document.createElement("a");
    link.href = doc.base64;
    link.download = doc.name || "documento";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const handleDeleteDocument = (doc: Adjunto) => {
    SweetAlertCustom({
      title: "¿Estás seguro?",
      description: `¿Deseas eliminar el documento ${doc.name}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí, continuar",
      cancelButtonText: "Cancelar",
      onConfirm: async () => {
        onDelete(doc.id).then((result) => {
          if (result.result) {
            ToastifyCustom({
              options: { autoClose: 2000, position: "bottom-right" },
              message: `El documento fue eliminado correctamente.`,
              type: "success",
            });
          } else {
            ToastifyCustom({
              message: result.message ? result.message[0] : "An error occurred",
              type: "error",
            });
          }
        });
      },
    });
  };

  const openForm = (doc?: Adjunto) => {
    ManageLocalStorage.saveToLocalStorage("idAdjunto", doc?.id ?? 0);
    setIsFormOpen(true);
  };

  const closeForm = () => {
    setIsFormOpen(false);
  };
  if (loading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <>
      <div className="mt-5 max-w-6xl mx-auto">
        {/* Seccion de Titulo y Boton de crear */}
        <div className="flex justify-between items-center mb-6">
          <h2 className="md:text-2xl sm:text-sm font-bold text-gray-800">
            Documentos del Estudiante
          </h2>
          <button
            className="bg-gray-900 border border-gray-900 h-10 w-10 text-white rounded-full flex items-center justify-center text-[14px] transition-all duration-200 ease-in-out hover:bg-white hover:text-gray-900"
            onClick={() => {
              ManageLocalStorage.removeFromLocalStorage("adjuntoEditing");
              ManageLocalStorage.removeFromLocalStorage("idAdjunto");
              openForm();
            }}
            title="Crear nuevo"
          >
            <FaPlus />
          </button>
        </div>

        {!loading &&
          (adjuntos.length === 0 ? (
            <div className="text-center py-10 text-gray-500">
              No se encontraron documentos para este estudiante
            </div>
          ) : (
            <div className="max-w-screen-xl mx-auto">
              <div className="flex flex-wrap gap-6 justify-start ">
                {adjuntos.map((doc) => (
                  <div
                    key={doc.id}
                    className="w-full sm:w-1/2 md:w-1/3 flex-grow"
                  >
                    <Card
                      descripcion={doc.description}
                      titulo={doc.name}
                      base64={doc.base64 ?? ""}
                      doc={doc}
                      downloadDocument={downloadDocument}
                      handleDeleteDocument={handleDeleteDocument}
                      openForm={openForm}
                      openDocumentViewer={openDocumentViewer}
                    />
                  </div>
                ))}
              </div>
            </div>
          ))}

        {/* Visor de documentos (igual para todos los tamaños) */}
        <DocumentViewerDialog
          isOpen={isViewerOpen}
          onClose={closeDocumentViewer}
          document={{
            name: selectedDoc?.name ?? "",
            base64: selectedDoc?.base64 ?? "",
          }}
          onDownload={() => downloadDocument(selectedDoc ?? undefined)}
        />

        {/* Renderizado condicional del formulario según el tamaño de pantalla */}
        {isFormOpen && isMobile ? (
          <DocumentoFormDialog isOpen={isFormOpen} onClose={closeForm} />
        ) : (
          <DocumentoFormDialog isOpen={isFormOpen} onClose={closeForm} />
        )}
        <div className="flex justify-center mt-6">
          <button
            type="button"
            onClick={() => navigate("/students", { replace: true })}
            className="w-32 px-4 py-2 bg-gray-900 text-white rounded-lg hover:bg-gray-800 transition-colors"
          >
            Regresar
          </button>
        </div>
      </div>
    </>
  );
};
