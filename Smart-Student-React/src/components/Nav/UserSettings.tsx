import { useState, useRef, useEffect } from "react";
import {
  RiEdit2Line,
  RiLockPasswordLine,
  RiSettings3Line,
  RiShutDownLine,
} from "react-icons/ri";
import { Link } from "react-router-dom";
import { SweetAlertChangePassword } from "./SweetAlertChangePassword";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { ToastifyCustom } from "../../util/ToastifyCustom ";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

interface UserSettingsProps {
  onClickChangePass?: () => void;
  onClickEditUser?: () => void;
  onLogOut?: () => void;
}

export const UserSettings = ({
  onClickChangePass,
  onClickEditUser,
  onLogOut,
}: UserSettingsProps) => {
  const onforgotPassword = useAuthStore((state) => state.forgotPassword);

  const [isOpen, setIsOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  const toggleMenu = () => setIsOpen(!isOpen);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div className="relative inline-block" ref={menuRef}>
      {/* Menú arriba */}
      {isOpen && (
        <div className="absolute right-[40px] bottom-[40px]  mb-2 w-48 bg-white rounded-md shadow-lg z-50">
          <ul className="py-1 text-sm text-gray-700">
            <li>
              <a
                href="/auth/login"
                onClick={() => {
                  setIsOpen(false);
                  if (onLogOut) {
                    onLogOut();
                  }
                }}
                className="block px-4 py-2 w-full text-left hover:bg-gray-100"
              >
                <span className="flex items-center gap-2">
                  <RiShutDownLine />
                  Cerrar Sesión
                </span>
              </a>
            </li>
            <li>
              <button
                onClick={() => {
                  setIsOpen(false);
                  if (onClickChangePass) {
                    onClickChangePass();
                  }
                  SweetAlertChangePassword({
                    onSubmit: (email) => {
                      showCustomLoading("");
                      onforgotPassword(email).then((result) => {
                        closeCustomLoading();
                        if (result.result) {
                          ToastifyCustom({
                            options: { autoClose: 2000 },
                            message: result.data ?? "Proceso correcto",
                            type: "success",
                          });
                        } else {
                          ToastifyCustom({
                            message: result.message ?? "An error occurred",
                            type: "error",
                          });
                        }
                      });
                    },
                  });
                }}
                className="block px-4 py-2 w-full text-left hover:bg-gray-100"
              >
                <span className="flex items-center gap-2">
                  <RiLockPasswordLine />
                  Cambiar contraseña
                </span>
              </button>
            </li>
            <li>
              <Link
                onClick={() => {
                  setIsOpen(false);
                  if (onClickEditUser) {
                    onClickEditUser();
                  }
                }}
                to="/user-login-edit"
                className="block px-4 py-2 w-full text-left hover:bg-gray-100"
              >
                <span className="flex items-center gap-2">
                  <RiEdit2Line />
                  Editar usuario
                </span>
              </Link>
            </li>
          </ul>
        </div>
      )}

      {/* Botón del ícono */}
      <button onClick={toggleMenu} className="absolute right-0 top-[-30px] ">
        <RiSettings3Line className="text-white text-[25px] ml-5 cursor-pointer" />
      </button>
    </div>
  );
};
