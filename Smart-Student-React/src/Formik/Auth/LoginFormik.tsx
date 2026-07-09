import * as Yup from "yup";

import { Formik, Form } from "formik";
import { LuLogIn } from "react-icons/lu";

import { CustomInput } from "../CustomInput";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { ToastifyCustom } from "../../util/ToastifyCustom ";
import { useNavigate } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

export const LoginFormik = () => {
  const navigate = useNavigate();

  const setLoading = useAuthStore((state) => state.setLoading);
  const loginRes = useAuthStore((state) => state.login);

  const validationSchema = Yup.object({
    password: Yup.string()
      .max(20, "Must be 20 characters or less")
      .required("Requerido"),
    email: Yup.string().email("Invalid email address").required("Requerido"),
  });

  return (
    <Formik
      initialValues={{
        email: "",
        password: "",
      }}
      validationSchema={validationSchema}
      onSubmit={async (values) => {
        setLoading(true);
        showCustomLoading();
        const targetResult = await loginRes(values);
        setLoading(false);
        closeCustomLoading();

        if (!targetResult.result) {
          ToastifyCustom({
            type: "error",
            message: targetResult.message ?? "",
          });
          return;
        }
        const lastUrl = localStorage.getItem("lastVisitedUrl") || "/";
        navigate(lastUrl);
        return;
      }}
    >
      {({ handleSubmit, setFieldValue }) => (
        <div>
          <Form onSubmit={handleSubmit} noValidate>
            <CustomInput
              name="email"
              placeholder="Correo Electrónico"
              type="email"
            />
            <CustomInput
              name="password"
              placeholder="Contraseña"
              type="password"
              customClassName="mt-5"
            />

            <button
              type="button"
              onClick={() => {
                setFieldValue("email", "admin@democampus.com");
                setFieldValue("password", "123Pass$$word!");
                ToastifyCustom({
                  type: "info",
                  message: "Credenciales de prueba autocompletadas ✅",
                });
              }}
              className="mt-6 text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors flex items-center justify-center w-full gap-2 border border-blue-100 py-2 rounded-lg bg-blue-50/50 hover:bg-blue-50"
            >
              <span>Usar credenciales de prueba</span>
            </button>

            <button
              type="submit"
              className="mt-4 tracking-wide font-semibold bg-gray-900 text-gray-100 w-full py-4 rounded-lg hover:bg-gray-800 transition-all duration-300 ease-in-out flex items-center justify-center focus:shadow-outline focus:outline-none"
            >
              <LuLogIn className="" />

              <span className="ml-3">Entrar</span>
            </button>
          </Form>
        </div>
      )}
    </Formik>
  );
};
