import { MdArrowBack } from "react-icons/md";

import forgotPassword from "../../assets/login/forgot-password_odai.svg";
import { Link } from "react-router-dom";
import { ForgotPasswordFormik } from "../../Formik/Auth/ForgotPasswordFormik";

export const ForgotPassword = () => {
  return (
    <>
      <div>
        <div className="min-h-screen bg-white text-gray-900 flex justify-center">
          <div className="max-w-screen-xl m-0 sm:m-10   flex justify-center flex-1">
            <div className="flex-1 text-center hidden lg:flex max-w-[650px]">
              <div
                className="m-12 xl:m-16 w-full bg-contain bg-center bg-no-repeat"
                style={{ backgroundImage: `url(${forgotPassword})` }}
              ></div>
            </div>

            <div className="lg:w-1/2 xl:w-5/12 p-0 sm:p-0 flex justify-start lg:justify-center flex-col ">
              <div className=" h-[200px] md:h-[300px] flex lg:hidden mt-10 mb-10 md:mt-5 md:mb-5 lg:mt-0 lg:mb-0 ">
                <div
                  className="  w-full bg-contain bg-center bg-no-repeat"
                  style={{ backgroundImage: `url(${forgotPassword})` }}
                ></div>
              </div>

              <div className="mt-12 flex flex-col items-center">
                <h1 className="text-2xl xl:text-3xl font-extrabold">
                  Recuperar Contraseña
                </h1>
                <div className=" max-w-[300px] text-center mt-5">
                  <p>
                    Por favor, introduzca su correo electrónico para recuperar
                    su contraseña
                  </p>
                </div>

                <div className="w-full flex-1 mt-5 ">
                  <div className="mx-auto max-w-xs md:p-0 md:mb-10 p-[7px] mb-10">
                    <ForgotPasswordFormik />

                    <div className="text-center mt-5 flex justify-center">
                      <Link
                        to="/auth/login"
                        className="text-blue-600 hover:text-blue-800 transition-colors duration-200  font-semibold flex items-center space-x-2"
                      >
                        <MdArrowBack />
                        <span>Volver al inicio</span>
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
