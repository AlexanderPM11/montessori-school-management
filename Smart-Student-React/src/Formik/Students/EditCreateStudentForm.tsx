import { useStudentStore } from "../../hooks/store/Student/Students.store";
import { Student } from "../../interfaces/Students/Student";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import {
  Formik,
  Form,
  useNavigate,
  useSearchParams,
  useGeneralStore,
  useEffect,
  useState,
  ManageLocalStorage,
  ToastifyCustom,
  ProfileUser,
  useUserStore,
} from "../Users/user";
import { BtnsForm } from "./BtnsForm";
import { FormFields } from "./FormFields";
import { validationSchema } from "./registrationSchema";

export const EditCreateStudentForm = () => {
  const isLoading = useGeneralStore((state) => state.loading);
  const getData = useGeneralStore((state) => state.onGetData);
  const getFathersAndMothers = useUserStore(
    (state) => state.getFathersAndMothers
  );

  // const isCreating = useGeneralStore((state) => state.isCreating);

  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const students = useStudentStore((state) => state.students);
  const onCreateOrUpdate = useStudentStore((state) => state.onCreateOrUpdate);

  const idStudent = Number(searchParams.get("idStudent"));
  const isParentsView =
    searchParams.get("isParentsView") === "true" ? true : false;

  let StudentSelect: Student | undefined | null;

  if (idStudent) {
    if (students.length > 0) {
      StudentSelect = students.find((x) => x.id === idStudent);
    }
    if (StudentSelect) {
      ManageLocalStorage.saveToLocalStorage("studentEditing", StudentSelect);
    } else {
      StudentSelect =
        ManageLocalStorage.readFromLocalStorage<Student>("studentEditing");
    }
  }

  const [imageProfileSelect, setImageProfile] = useState<string | null>(
    StudentSelect?.urlImg ?? ""
  );
  const onSetImageProfile = (image: string | null) => {
    setImageProfile(image);
  };
  const [imageFile, setImageFile] = useState<File | null>(null);
  const dateBirth = StudentSelect?.bornDate?.toString() || "";
  const onlyDate = dateBirth ? dateBirth.split("T")[0] : "";

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });

    getFathersAndMothers();
    getData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // User Info
  const userInfo = {
    id: StudentSelect?.id || "",
    name: StudentSelect?.name || "",
    lastname: StudentSelect?.lastname || "",
    sexo: StudentSelect?.sexo?.toString() || "",
    noBook: StudentSelect?.book || "",
    noFolio: StudentSelect?.folio || "",
    bornDate: onlyDate,
    direction: StudentSelect?.direction || "",
    telFather: StudentSelect?.telFather || "",
    telMother: StudentSelect?.telMother || "",
    idRoom: StudentSelect?.idRoom || "",
    idTypeRegister: StudentSelect?.idTypeRegister.toString() || "",
    tel: StudentSelect?.tel || "",
    relationship: StudentSelect?.relationPersonLiveWith || "",
    carriedPreprimary: StudentSelect?.carriedPreprimary.toString() || "false",
    neae: StudentSelect?.neae || "",
    diseasesAllergic: StudentSelect?.diseasesAllergic || "",
    medicinesUse: StudentSelect?.medicinesUse || "",
    emergencyPerson: StudentSelect?.emergencyPerson || "",
    emergencyTel: StudentSelect?.emergencyTel || "",
    idFather: StudentSelect?.idFather || "",
    idMother: StudentSelect?.idMother || "",
    idNacionality: StudentSelect?.idNacionality?.toString() || "",
    numberSiblings: StudentSelect?.numberSiblings || "",
    agesSiblings: StudentSelect?.agesSiblings || "",
    placeBetweenSiblings: StudentSelect?.placeBetweenSiblings || "",
    doctorPediatrician: StudentSelect?.doctorPediatrician || "",
    urlImg: StudentSelect?.urlImg || "",
    idGrade: StudentSelect?.idGrade.toString() || "0",

    file: StudentSelect?.file,
  };
  const handleImageFileSelected = (file: File) => {
    setImageFile(file); // Guardamos el archivo para su uso posterior (envío, vista previa, etc.)
  };

  const handleUserSubmit = async (values: typeof userInfo) => {
    const userToSave = {
      id: Number(values?.id) || 0,
      name: values?.name || "",
      lastname: values?.lastname || "",
      sexo: values?.sexo?.toString() || "",
      book: values?.noBook || "",
      folio: values?.noFolio || "",
      bornDate: new Date(values.bornDate).toISOString(),
      direction: values?.direction || "",
      telFather: values?.telFather || "",
      telMother: values?.telMother || "",
      idRoom: values?.idRoom || "",
      idTypeRegister: Number(values?.idTypeRegister) || 0,
      tel: values?.tel || "",
      carriedPreprimary: values?.carriedPreprimary === "true" ? true : false,
      neae: values?.neae || "",
      diseasesAllergic: values?.diseasesAllergic || "",
      medicinesUse: values?.medicinesUse || "",
      emergencyPerson: values?.emergencyPerson || "",
      emergencyTel: values?.emergencyTel || "",
      idFather: values?.idFather || "",
      idMother: values?.idMother || "",
      idNacionality: Number(values?.idNacionality) || 0,
      numberSiblings: values?.numberSiblings || "",
      agesSiblings: values?.agesSiblings || "",
      placeBetweenSiblings: values?.placeBetweenSiblings || "",
      doctorPediatrician: values?.doctorPediatrician || "",
      urlImg: imageProfileSelect ?? "",
      idGrade: Number(values?.idGrade) || 0,
      relationPersonLiveWith: values?.relationship || "0",
      code: StudentSelect?.code || null,
      file: imageFile ?? undefined,
    };
    const mergedUser = {
      ...(StudentSelect as Student),
      ...userToSave,
    };
    showCustomLoading();
    const result = await onCreateOrUpdate(mergedUser);
    closeCustomLoading();
    if (result.result) {
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Datos actualizados exitosamente",
        type: "success",
      });
      if (isParentsView) {
        navigate(-1);
      } else {
        navigate("/students", { replace: true });
      }
    } else {
      ToastifyCustom({
        message: result.message ?? "An error occurred",
        type: "error",
      });
    }
  };
  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  const navigateLink = isParentsView ? null : "/students";
  return (
    <div className="mt-5">
      <Formik
        initialValues={userInfo}
        validationSchema={validationSchema}
        onSubmit={handleUserSubmit}
        enableReinitialize
      >
        {({ handleSubmit }) => (
          <div>
            <Form onSubmit={handleSubmit} noValidate>
              {/* Profile User Component */}
              <ProfileUser
                imageProfileSelect={imageProfileSelect}
                setImageProfile={onSetImageProfile}
                onloadend={handleImageFileSelected}
              />
              {/* Campos del formulario */}
              <FormFields />
              {/* Botones al final del formulario */}
              <BtnsForm navigateLink={navigateLink} />
            </Form>
          </div>
        )}
      </Formik>
    </div>
  );
};
