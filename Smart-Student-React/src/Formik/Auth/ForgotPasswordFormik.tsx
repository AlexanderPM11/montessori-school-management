import * as Yup from "yup";

import { Formik, Form } from "formik";

import { CustomInput } from "../CustomInput";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { ToastifyCustom } from "../../util/ToastifyCustom ";
import { GrLinkNext } from "react-icons/gr";
import { useNavigate } from "react-router-dom";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

export const ForgotPasswordFormik = () => {
  const navigate = useNavigate();

  const isLoading = useAuthStore((state) => state.loading);
  const setLoading = useAuthStore((state) => state.setLoading);
  const forgotPassword = useAuthStore((state) => state.forgotPassword);

  const validationSchema = Yup.object({
    email: Yup.string().email("Invalid email address").required("Requerido"),
  });

  const onSubmitForm = async (email: string) => {
    setLoading(true);
    const targetResult = await forgotPassword(email);
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
      }}
      validationSchema={validationSchema}
      onSubmit={async (values) => {
        onSubmitForm(values.email);
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
            <button
              type="submit"
              className="mt-5 tracking-wide font-semibold bg-gray-900 text-gray-100 w-full py-4 rounded-lg hover:bg-gray-800 transition-all duration-300 ease-in-out flex items-center justify-center focus:shadow-outline focus:outline-none"
            >
              <GrLinkNext className="" />

              <span className="ml-3">Enviar</span>
            </button>
          </Form>
        </div>
      )}
    </Formik>
  );
};
