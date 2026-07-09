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
  User,
  ToastifyCustom,
  FormFields,
  BtnsForm,
  validationSchema,
  ProfileUser,
  useUserParentsStore,
} from "./user";

export const EditCreateuserParentsForm = () => {
  const onCreateEdit = useGeneralStore((state) => state.onGetData);
  const isLoading = useGeneralStore((state) => state.loading);
  const isCreating = useGeneralStore((state) => state.isCreating);

  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const users = useUserParentsStore((state) => state.imutableUsers);
  const onCreateOrUpdate = useUserParentsStore(
    (state) => state.onCreateOrUpdate
  );

  const idUser = searchParams.get("idUser");

  let userSelect: User | undefined | null;

  if (idUser) {
    if (users.length > 0) {
      userSelect = users.find((x) => x.id === idUser);
    }
    if (userSelect) {
      ManageLocalStorage.saveToLocalStorage("userEditingParents", userSelect);
    } else {
      userSelect =
        ManageLocalStorage.readFromLocalStorage<User>("userEditingParents");
    }
  }

  const [imageProfileSelect, setImageProfile] = useState<string | null>(
    userSelect?.urlImage ?? ""
  );

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
    };

    const mergedUser = {
      ...(userSelect as User),
      ...userToSave,
    };
    showCustomLoading();
    const result = await onCreateOrUpdate(mergedUser);
    closeCustomLoading();
    if (result.result) {
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Proceso correcto",
        type: "success",
      });
      navigate("/parents-users", { replace: true });
    } else {
      ToastifyCustom({
        message: result.message ?? "An error occurred",
        type: "error",
      });
    }
  };
  const [imageFile, setImageFile] = useState<File | null>(null);
  const handleImageFileSelected = (file: File) => {
    setImageFile(file); // Guardamos el archivo para su uso posterior (envío, vista previa, etc.)
  };
  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  return (
    <div className="lg:mt-5">
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
                setImageProfile={setImageProfile}
                onloadend={handleImageFileSelected}
              />
              {/* Campos del formulario */}
              <FormFields />
              {/* Botones al final del formulario */}
              <BtnsForm navigateLink="/parents-users" />
            </Form>
          </div>
        )}
      </Formik>
    </div>
  );
};
