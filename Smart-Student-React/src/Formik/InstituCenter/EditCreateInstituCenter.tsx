import { ImageSelector } from "../../components/ImageSelector";
import { useInstituCentersStore } from "../../hooks/store/InstituCenter/InstituCenter.store";
import { EducationalInstitutionViewModel } from "../../interfaces/IntiituCenter/EducationalInstitutionViewModel";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import {
  Formik,
  Form,
  useEffect,
  useState,
  ToastifyCustom,
} from "../Users/user";
import { BtnsForm } from "./BtnsForm";

import { FormFields } from "./FormFields";
import { validationSchema } from "./registrationSchema";
interface Props {
  onSave: () => void;
}
export const EditCreateInstituCenter = ({ onSave }: Props) => {
  const institutionStored = useInstituCentersStore(
    (state) => state.instituCenters
  );
  const onCreateOrUpdate = useInstituCentersStore(
    (state) => state.onCreateOrUpdate
  );

  const [imageProfileSelect, setImageProfile] = useState<string | null>(
    institutionStored?.urlLogo ?? ""
  );
  const [imageFile, setImageFile] = useState<File | null>(null);

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const initialValues: EducationalInstitutionViewModel = {
    id: institutionStored?.id ?? "",
    name: institutionStored?.name ?? "",
    address: institutionStored?.address ?? "",
    nameMunicipality: institutionStored?.nameMunicipality ?? "",
    phone: institutionStored?.phone ?? "",
    mobile: institutionStored?.mobile ?? "",
    idRector: institutionStored?.idRector ?? "",
    idCordinator: institutionStored?.idCordinator ?? "",
    idSecretary: institutionStored?.idSecretary ?? "",
    idProvinceDom: institutionStored?.idProvinceDom,
    idDepartment: institutionStored?.idDepartment ?? undefined,
    academicResolution: institutionStored?.academicResolution ?? "",
    educationalRegistry: institutionStored?.educationalRegistry ?? "",
    website: institutionStored?.website ?? "",
    urlLogo: institutionStored?.urlLogo ?? "",
    isMainSchool: institutionStored?.isMainSchool ?? false,
    idUser: institutionStored?.idUser ?? "",
    userAssignmentId: institutionStored?.userAssignmentId ?? "",
    session: institutionStored?.session ?? "",
    regional: institutionStored?.regional ?? "",
    district: institutionStored?.district ?? "",
    nameRector: institutionStored?.nameRector ?? "",
    nameCordinator: institutionStored?.nameCordinator ?? "",
    nameSecretary: institutionStored?.nameSecretary ?? "",
    nameAdmin: institutionStored?.nameAdmin ?? "",
    base64Img: institutionStored?.base64Img ?? "",
    file: undefined,
  };

  const handleImageFileSelected = (file: File) => {
    setImageFile(file);
  };

  const handleSubmit = async (values: EducationalInstitutionViewModel) => {
    const institutionToSave: EducationalInstitutionViewModel = {
      ...values,
      urlLogo: imageProfileSelect ?? "",
      file: imageFile ?? undefined,
    };

    showCustomLoading();
    const result = await onCreateOrUpdate(institutionToSave);
    closeCustomLoading();

    if (result.result) {
      onSave();
      ToastifyCustom({
        message: result.message ?? "Datos guardados correctamente",
        type: "success",
        options: { autoClose: 2000 },
      });
    } else {
      ToastifyCustom({
        message: result.message ?? "Ocurrió un error al guardar",
        type: "error",
      });
    }
  };

  return (
    <div className="mt-5 md:mb-16">
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
        enableReinitialize
      >
        {({ handleSubmit }) => (
          <Form onSubmit={handleSubmit} noValidate>
            <ImageSelector
              label="Logo del Centro Educativo"
              image={imageProfileSelect}
              onChangeImage={(base64, file) => {
                setImageProfile(base64);
                if (file) handleImageFileSelected(file);
              }}
              aspect="square"
            />
            <div className="mb-5"></div>
            <FormFields />
            <BtnsForm />
          </Form>
        )}
      </Formik>
    </div>
  );
};
