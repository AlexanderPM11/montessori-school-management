import { useEffect, useState } from "react";
import { Formik } from "formik";
import { Form, useNavigate, useSearchParams } from "react-router-dom";
import { FormFields } from "./FormFields";
import { FileUploader } from "../../components/FileUploader";
import { ManageLocalStorage, ToastifyCustom } from "../Users/user";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { validationSchema } from "./registrationSchema";
import { BtnsForm } from "./BtnsForm";
import { useSubjectStore } from "../../hooks/store/Subject/Subject.store";
import { Subject } from "../../interfaces/Subject/Subject";

export const CreateOrEditForm = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const idSubject = Number(searchParams.get("idSubject"));

  const [file, setFile] = useState<File>();
  const [base64, setBase64] = useState<string>("");

  const onCreateOrUpdate = useSubjectStore((state) => state.onCreateOrUpdate);
  const subjects = useSubjectStore((state) => state.subjects);

  let subjectselect: Subject | undefined | null;

  if (idSubject) {
    if (subjects.length > 0) {
      subjectselect = subjects.find((x) => x.id === idSubject);
    }
    if (subjectselect) {
      ManageLocalStorage.saveToLocalStorage("subjectEditing", subjectselect);
    } else {
      subjectselect =
        ManageLocalStorage.readFromLocalStorage<Subject>("subjectEditing");
    }
  }

  const subjectInfo = {
    name: subjectselect?.name || "",
    description: subjectselect?.description || "",
  };
  const initialBase64 = subjectselect?.imageUrl;

  const handleUserSubmit = async (values: typeof subjectInfo) => {
    const subjectToSave = {
      id: subjectselect?.id || 0,
      name: values?.name || "",
      description: values?.description || "",
      code: subjectselect?.code || "",
      imageUrl: base64 ?? subjectselect?.imageUrl,

      file: file || (subjectselect?.file as File),
    };

    const mergedUser = {
      ...(subjectInfo as Subject),
      ...subjectToSave,
    };
    showCustomLoading();
    const result = await onCreateOrUpdate(mergedUser);
    closeCustomLoading();
    if (result.result) {
      navigate("/subjects");
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
        initialValues={subjectInfo}
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
                  accept="image/*"
                  onFileSelect={(fileLoad, base) => {
                    if (fileLoad) {
                      setFile(fileLoad);
                      setBase64(base);
                    }
                  }}
                  initialBase64={initialBase64 ?? ""}
                />
              </div>

              {/* Botones al final del formulario */}
              <BtnsForm />
            </Form>
          </div>
        )}
      </Formik>
    </div>
  );
};
