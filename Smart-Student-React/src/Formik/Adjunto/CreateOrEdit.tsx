import { useEffect, useState } from "react";
import { Formik } from "formik";
import { Form, useSearchParams } from "react-router-dom";
import { FormFields } from "./FormFields";
import { validationSchema } from "./registrationSchema";
import { FileUploader } from "../../components/FileUploader";
import { useAdjuntoStore } from "../../hooks/store/Adjunto/Adjunto.store";
import { Adjunto } from "../../interfaces/Adjunto/Ajunto";
import { ManageLocalStorage, ToastifyCustom } from "../Users/user";
import { BtnsForm } from "./BtnsForm";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

interface CreateOrEditFormProps {
  onClose: () => void;
}

export const CreateOrEditForm = ({ onClose }: CreateOrEditFormProps) => {
  const [searchParams] = useSearchParams();

  const idStudent = Number(searchParams.get("idStudent"));

  const [file, setFile] = useState<File>();
  const [base64, setBase64] = useState<string>("");

  const onCreateOrUpdate = useAdjuntoStore((state) => state.onCreateOrUpdate);
  const adjuntos = useAdjuntoStore((state) => state.adjuntos);

  const idAdjunto =
    ManageLocalStorage.readFromLocalStorage<number>("idAdjunto");

  let adjuntoSelect: Adjunto | undefined | null;

  if (idAdjunto) {
    if (adjuntos.length > 0) {
      adjuntoSelect = adjuntos.find((x) => x.id === idAdjunto);
    }
    if (adjuntoSelect) {
      ManageLocalStorage.saveToLocalStorage("adjuntoEditing", adjuntoSelect);
    } else {
      adjuntoSelect =
        ManageLocalStorage.readFromLocalStorage<Adjunto>("adjuntoEditing");
    }
  }

  const adjuntoInfo = {
    name: adjuntoSelect?.name || "",
    base64: adjuntoSelect?.base64 || "",
    description: adjuntoSelect?.description || "",
    path: adjuntoSelect?.path || "",
    idTipoAdjunto: adjuntoSelect?.idTipoAdjunto || 0,
    idStudent: adjuntoSelect?.idStudent || idStudent || 0,
  };
  const initialBase64 = adjuntoInfo.base64;

  const handleUserSubmit = async (values: typeof adjuntoInfo) => {
    const adjuntoToSave = {
      id: adjuntoSelect?.id || 0,
      idStudent: values?.idStudent || 0,
      name: values?.name || "",
      description: values?.description || "",
      file: file || (adjuntoSelect?.file as File),
      base64: base64 || adjuntoSelect?.base64 || "",
      path: values.path || "",
      idTipoAdjunto: values.idTipoAdjunto || 0,
    };

    const mergedUser = {
      ...(adjuntoInfo as Adjunto),
      ...adjuntoToSave,
    };
    showCustomLoading();
    const result = await onCreateOrUpdate(mergedUser);
    closeCustomLoading();
    if (result.result) {
      onClose();
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Datos actualizados exitosamente",
        type: "success",
      });
    } else {
      ToastifyCustom({
        message: result.message ?? "An error occurred",
        type: "error",
      });
    }
  };
  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);
  return (
    <div className="md:px-5 px-4  md:pb-0 bg-white">
      <Formik
        initialValues={adjuntoInfo}
        validationSchema={validationSchema}
        onSubmit={handleUserSubmit}
        enableReinitialize
      >
        {({ handleSubmit }) => (
          <div>
            <Form onSubmit={handleSubmit} noValidate className="pb-10">
              {/* Campos del formulario */}
              <FormFields />
              {/*  */}
              <div className="mt-4">
                <FileUploader
                  onFileSelect={(fileLoad, base) => {
                    if (fileLoad) {
                      setFile(fileLoad);
                      setBase64(base);
                    }
                  }}
                  initialBase64={initialBase64}
                />
              </div>

              {/* Botones al final del formulario */}
              <BtnsForm onClose={onClose} />
            </Form>
          </div>
        )}
      </Formik>
    </div>
  );
};
