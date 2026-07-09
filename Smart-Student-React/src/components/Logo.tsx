import { useState } from "react";
import { Link } from "react-router-dom";
import logo2 from "../assets/images/logo.png";
import { useAuthStore } from "../hooks/store/Auth.store";
import { FaInfoCircle, FaCopy, FaCheck } from "react-icons/fa";

interface Props {
  showLabel?: boolean;
}

export const Logo = ({ showLabel = true }: Props) => {
  const userLoggued = useAuthStore((state) => state.UserLoggued);
  const roles = userLoggued?.roles || [];
  const [showDemoInfo, setShowDemoInfo] = useState(false);
  const [copied, setCopied] = useState<string | null>(null);

  const getRedirectPath = () => {
    if (roles.includes("Admin") || roles.includes("Coordinador")) {
      return "/home";
    }
    if (roles.includes("Profesor")) {
      return "/rooms";
    }
    return "/home";
  };

  const copyToClipboard = (text: string, field: string) => {
    navigator.clipboard.writeText(text);
    setCopied(field);
    setTimeout(() => setCopied(null), 2000);
  };

  const demoUser = {
    UserName: "AdminUser",
    Password: "TuPasswordSeguro!123", // Asumiendo el pass del .env
    FirstName: "Alexander",
    LastName: "Polanco Moreno",
    Email: "adminuser@gmail.com",
    PhoneNumber: "809-778-7886",
    Address: "Monte Plata",
    IdentificationId: "000-000000-0",
  };

  return (
    <div className="relative flex items-center space-x-2">
      <Link
        to={getRedirectPath()}
        className="flex items-center space-x-2 text-white hover:opacity-90 transition-opacity"
      >
        <img
          src={logo2}
          alt="SmartSystem Logo"
          className="h-12 w-auto object-contain"
        />
        {showLabel && (
          <div className="w-auto min-w-[140px]">
            <span className="text-[1.12rem] md:text-xl font-semibold tracking-wide">
              Smart System
            </span>
          </div>
        )}
      </Link>

      {!userLoggued && (
        <div className="relative">
          <button
            onMouseEnter={() => setShowDemoInfo(true)}
            onMouseLeave={() => setShowDemoInfo(false)}
            onClick={() => setShowDemoInfo(!showDemoInfo)}
            className="text-blue-400 hover:text-white transition-colors p-1"
            title="Credenciales de Prueba"
          >
            <FaInfoCircle className="text-xl" />
          </button>

          {showDemoInfo && (
            <div 
              onMouseEnter={() => setShowDemoInfo(true)}
              onMouseLeave={() => setShowDemoInfo(false)}
              className="absolute left-0 top-full mt-2 w-72 bg-gray-800/95 backdrop-blur-md border border-gray-700 rounded-xl shadow-2xl p-4 z-[2000] text-sm text-gray-200"
            >
              <div className="mb-3 border-b border-gray-700 pb-2">
                <h3 className="font-bold text-blue-400">Acceso de Prueba</h3>
                <p className="text-[10px] text-gray-400">Alexander Polanco Moreno</p>
              </div>
              
              <div className="space-y-3">
                <div className="flex justify-between items-center group">
                  <div>
                    <label className="text-[10px] text-gray-500 uppercase font-bold block">Usuario</label>
                    <span className="font-mono text-xs">{demoUser.UserName}</span>
                  </div>
                  <button 
                    onClick={() => copyToClipboard(demoUser.UserName, 'user')}
                    className="p-1.5 hover:bg-gray-700 rounded transition-colors"
                  >
                    {copied === 'user' ? <FaCheck className="text-green-500" /> : <FaCopy className="text-gray-500" />}
                  </button>
                </div>

                <div className="flex justify-between items-center group">
                  <div>
                    <label className="text-[10px] text-gray-500 uppercase font-bold block">Contraseña</label>
                    <span className="font-mono text-xs">TuPasswordSeguro!123</span>
                  </div>
                  <button 
                    onClick={() => copyToClipboard('TuPasswordSeguro!123', 'pass')}
                    className="p-1.5 hover:bg-gray-700 rounded transition-colors"
                  >
                    {copied === 'pass' ? <FaCheck className="text-green-500" /> : <FaCopy className="text-gray-500" />}
                  </button>
                </div>

                <div className="pt-2 border-t border-gray-700 mt-2">
                  <div className="grid grid-cols-2 gap-2 text-[10px]">
                    <div>
                      <p className="text-gray-500 font-bold uppercase">Identificación</p>
                      <p>{demoUser.IdentificationId}</p>
                    </div>
                    <div>
                      <p className="text-gray-500 font-bold uppercase">Teléfono</p>
                      <p>{demoUser.PhoneNumber}</p>
                    </div>
                    <div className="col-span-2">
                      <p className="text-gray-500 font-bold uppercase">Email</p>
                      <p>{demoUser.Email}</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
};
