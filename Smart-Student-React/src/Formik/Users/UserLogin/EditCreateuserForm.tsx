import { Form } from "react-router-dom";
import {
  BtnsForm,
  ProfileUser,
  ToastifyCustom,
  useEffect,
  useGeneralStore,
  useNavigate,
  User,
  useState,
} from "../user";
import { validationSchema } from "./registrationSchema";
import { FormFields } from "./FormFields";
import { Formik } from "formik";
import { useAuthStore } from "../../../hooks/store/Auth.store";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";

export const EditUserLoginForm = () => {
  const userSelect = useAuthStore((state) => state.UserLoggued);

  const onCreateEdit = useGeneralStore((state) => state.onGetData);
  const isLoading = useGeneralStore((state) => state.loading);
  const isCreating = useGeneralStore((state) => state.isCreating);

  const navigate = useNavigate();
  const onUpdateUserLogin = useAuthStore((state) => state.onUpdateUserLogin);

  const [imageProfileSelect, setImageProfile] = useState<string | null>(
    userSelect?.urlImage ?? ""
  );
  const onSetImageProfile = (image: string | null) => {
    setImageProfile(image);
  };

  const dateBirth = userSelect?.dateBirth?.toString() || "";
  const onlyDate = dateBirth ? dateBirth.split("T")[0] : "";

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });

    onCreateEdit();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // User Info
  const userInfo = {
    id: userSelect?.id || "",
    firstName: userSelect.firstName,
    lastName: userSelect.lastName,
    name: userSelect?.firstName || "",
    password: "",
    confirmPassword: "",
    lastname: userSelect?.lastName || "",
    userName: userSelect?.userName || "",
    cardId: userSelect?.identificationId || "",
    noBook: userSelect?.noBook || "",
    noFolio: userSelect?.noFolio || "",
    gender: userSelect?.gender?.toString() || "",
    dateBirth: onlyDate,
    nationality: userSelect?.nationality || "",
    educationLevel: userSelect?.idNivelEducativo || "",
    relationship: userSelect?.relationshipId || "",
    email: userSelect?.email || "",
    tel: userSelect?.tel || "",
    phone: userSelect?.phoneNumber || "",
    addres: userSelect?.addres || "",
    profession: userSelect?.profession || "",
    job: userSelect?.job || "",
    placeJob: userSelect?.placeWork || "",
    civilStatus: userSelect?.civilStatus || "",
    profileImage: userSelect?.urlImage || "",
    roles: userSelect?.roles || [],
  };
  const [imageFile, setImageFile] = useState<File | null>(null);
  const handleImageFileSelected = (file: File) => {
    setImageFile(file); // Guardamos el archivo para su uso posterior (envío, vista previa, etc.)
  };
  const handleUserSubmit = async (values: typeof userInfo) => {
    const userToSave = {
      id: values.id,
      firstName: values.name,
      lastName: values.lastname,
      userName: values.userName,
      identificationId: values.cardId,
      noBook: values.noBook,
      noFolio: values.noFolio,
      gender: parseInt(values.gender),
      dateBirth: new Date(values.dateBirth).toISOString(),
      nationality: values.nationality,
      idNivelEducativo: values.educationLevel,
      relationshipId: values.relationship,
      email: values.email,
      tel: values.tel,
      phoneNumber: values.phone,
      addres: values.addres,
      profession: values.profession,
      job: values.job,
      placeWork: values.placeJob,
      civilStatus: values.civilStatus,
      file: imageFile ?? undefined,
      urlImage: imageProfileSelect ?? "",
      worksAnActivityDiferentThanteaching: isCreating
        ? false
        : userSelect?.worksAnActivityDiferentThanteaching ?? false,
      estado: isCreating ? true : userSelect?.estado ?? true,
      statu: isCreating ? true : userSelect?.statu ?? true,
      roles: values.roles,
      password: values.password,
      token: userSelect?.token,
    };
    const mergedUser = {
      ...(userSelect as User),
      ...userToSave,
    };
    showCustomLoading();

    const result = await onUpdateUserLogin(mergedUser);
    closeCustomLoading();

    if (result.result) {
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Proceso correcto",
        type: "success",
      });
      navigate("/administractive-users", { replace: true });
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
  return (
    <div className="md:px-5 mt-20">
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
              <BtnsForm />
            </Form>
          </div>
        )}
      </Formik>
    </div>
  );
};
