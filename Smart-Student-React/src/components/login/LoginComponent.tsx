import { Link } from "react-router-dom";
import loginSVG from "../../assets/login/login.svg";
import { Logo } from "../Logo";

import { LoginFormik } from "../../Formik/Auth/LoginFormik";

export const LoginComponent = () => {
  return (
    <>
      <div>
        <div className="min-h-screen bg-white text-gray-900 flex justify-center">
          <div className="max-w-screen-xl m-0  flex justify-center flex-1">
            <div className="flex-1 text-center hidden lg:flex">
              <div
                className="m-12 xl:m-16 w-full bg-contain bg-center bg-no-repeat"
                style={{ backgroundImage: `url(${loginSVG})` }}
              ></div>
            </div>

            <div className="lg:w-1/2 xl:w-5/12 p-0 sm:p-0 flex justify-start lg:justify-center flex-col ">
              <div className="h-[200px] md:h-[400px] flex lg:hidden  lg:mt-0 lg:mb-0">
                <div
                  className="  w-full bg-contain bg-center bg-no-repeat"
                  style={{ backgroundImage: `url(${loginSVG})` }}
                ></div>
              </div>

              <div className="mt-8 flex flex-col items-center">
                <div className="mb-6 scale-125 md:scale-150 py-4">
                  <Logo showLabel={false} />
                </div>
                <h1 className="text-2xl xl:text-3xl font-extrabold text-gray-800">
                  Iniciar Sesión
                </h1>

                <div className="w-full flex-1 mt-8 ">
                  <div className="mx-auto max-w-xs md:p-0 md:mb-10 min-w-[320px] p-3 mb-10">
                    <LoginFormik />

                    <div className="text-center mt-5">
                      <Link
                        to="/auth/forgot-password"
                        className="text-blue-600 hover:text-blue-800 transition-colors duration-200 "
                        aria-label="¿Olvidó su contraseña?"
                      >
                        ¿Olvidó su contraseña?
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
