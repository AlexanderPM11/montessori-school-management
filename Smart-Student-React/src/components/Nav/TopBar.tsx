import { FiCalendar, FiBell } from "react-icons/fi";
import { BiUserCircle } from "react-icons/bi";
import { useAuthStore } from "../../hooks/store/Auth.store";
import { useState } from "react";
import { DialogCusmtom } from "../../util/DialogCusmtom";

export const TopBar = () => {
  const userLoggued = useAuthStore((state) => state.UserLoggued);
  const [isDialogOpen, setIsDialogOpen] = useState(false);

  const today = new Date().toLocaleDateString("es-DO", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
  });

  const systemStatus = "Online";
  const userName = `${userLoggued.firstName?.trim() || ""} ${
    userLoggued.lastName
      ? userLoggued.lastName.charAt(0).toUpperCase() + "."
      : ""
  }`.trim();

  return (
    <>
      <div className="fixed justify-end top-0 left-0 right-0 h-16 bg-white text-gray-800 z-20 flex items-center px-6 shadow-md">
        <div className="flex items-center space-x-4 text-sm">
          {/* Usuario con estado */}
          <div className="flex items-center gap-2 bg-gray-100 px-3 py-1 rounded-full">
            <BiUserCircle className="w-5 h-5 text-green-500" />
            <span className="flex items-center gap-2">
              {userName}
              <span
                className={`w-3 h-3 rounded-full inline-block ${
                  systemStatus === "Online" ? "bg-green-500" : "bg-red-500"
                }`}
                title={systemStatus}
              ></span>
            </span>
          </div>

          {/* Fecha */}
          <div className="flex items-center gap-2 bg-gray-100 px-3 py-1 rounded-full">
            <FiCalendar className="w-5 h-5 text-yellow-500" />
            {today}
          </div>

          {/* Notificaciones */}
          <div className="relative">
            <button
              onClick={() => setIsDialogOpen(true)}
              className="bg-gray-100 hover:bg-gray-200 transition px-3 py-1 rounded-lg flex items-center gap-1"
            >
              <FiBell className="w-5 h-5 text-gray-700" />
              Notificaciones
            </button>

            {/* Badge de notificaciones */}
            <span className="absolute -top-2 -right-2 inline-flex items-center justify-center w-5 h-5 text-xs font-bold leading-none text-white bg-red-500 rounded-full">
              0
            </span>
          </div>
        </div>
      </div>

      {/* Dialog */}
      <DialogCusmtom
        isOpen={isDialogOpen}
        onClose={() => setIsDialogOpen(false)}
        title="Notificaciones"
      >
        <p className="font-bold text-center">
          No hay notificaciones disponibles.
        </p>
      </DialogCusmtom>
    </>
  );
};
