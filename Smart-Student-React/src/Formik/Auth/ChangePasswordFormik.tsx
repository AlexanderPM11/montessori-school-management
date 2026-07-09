import * as Yup from "yup";

import { Formik, Form } from "formik";

import { CustomInput } from "../CustomInput";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { ToastifyCustom } from "../../util/ToastifyCustom ";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { MdArrowBack, MdOutlinePublishedWithChanges } from "react-icons/md";
import { ChangePasswordInterface } from "../../interfaces/Auth/ChangePasswordInterface";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

export const ChangePasswordFormik = () => {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token") ?? "";

  const navigate = useNavigate();

  const isLoading = useAuthStore((state) => state.loading);
  const setLoading = useAuthStore((state) => state.setLoading);
  const changePassword = useAuthStore((state) => state.changePassword);

  const validationSchema = Yup.object({
    email: Yup.string().email("Invalid email address").required("Requerido"),
    password: Yup.string()
      .required("La contraseña es requerida.")
      .min(8, "La contraseña debe tener al menos 8 caracteres.")
      .max(20, "La contraseña no puede tener más de 20 caracteres.")
      .matches(
        /[A-Z]/,
        "La contraseña debe contener al menos una letra mayúscula."
      )
      .matches(
        /[a-z]/,
        "La contraseña debe contener al menos una letra minúscula."
      )
      .matches(/\d/, "La contraseña debe contener al menos un número.")
      .matches(
        /[!@#$%^&*(),.?":{}|<>]/,
        "La contraseña debe contener al menos un carácter especial (por ejemplo: !@#$%^&*)."
      ),
    confirmPassword: Yup.string()
      .required("La confirmación de la contraseña es requerida.")
      .oneOf([Yup.ref("password"), ""], "Las contraseñas deben coincidir."),
  });

  const onSubmitForm = async (values: ChangePasswordInterface) => {
    setLoading(true);
    const targetResult = await changePassword(values);
    setLoading(false);

    if (!targetResult.result) {
      ToastifyCustom({
        type: "error",
        message: targetResult.message ?? "",
      });
    } else {
      navigate("/auth/login");

      ToastifyCustom({
        type: "success",
        message: targetResult.data ?? "",
      });
    }
  };
  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  return (
    <Formik
      initialValues={{
        email: "",
        password: "",
        confirmPassword: "",
        token: token,
      }}
      validationSchema={validationSchema}
      onSubmit={async (values) => {
        onSubmitForm(values);
      }}
    >
      {({ handleSubmit }) => (
        <div>
          <Form onSubmit={handleSubmit} noValidate>
            <CustomInput
              name="email"
              placeholder="Correo Electrónico"
              type="email"
            />
            <CustomInput
              name="password"
              placeholder="Contraseña Nueva"
              type="password"
              customClassName="mt-5"
            />
            <CustomInput
              name="confirmPassword"
              placeholder="Confirmar Contraseña"
              type="password"
              customClassName="mt-5"
            />

            <button className="mt-5 tracking-wide font-semibold bg-gray-900 text-gray-100 w-full py-4 rounded-lg hover:bg-gray-800 transition-all duration-300 ease-in-out flex items-center justify-center focus:shadow-outline focus:outline-none">
              <MdOutlinePublishedWithChanges className="" />

              <span className="ml-3">Cambiar</span>
            </button>

            <div className="text-center mt-5 flex justify-center">
              <Link
                to="/auth/login"
                className="text-blue-600 hover:text-blue-800 transition-colors duration-200 font-semibold flex items-center space-x-2"
              >
                <MdArrowBack />
                <span>Volver al inicio</span>
              </Link>
            </div>
          </Form>
        </div>
      )}
    </Formik>
  );
};
