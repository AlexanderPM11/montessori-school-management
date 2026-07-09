
import {
  FaSchool,
  FaMapMarkerAlt,
  FaEdit,
  FaInfoCircle,
  FaUniversity,
  FaRegBuilding,
  FaClipboardCheck,
  FaPhone,
  FaMobileAlt,
  FaGlobe,
  FaUserTie,
  FaChalkboardTeacher,
  FaIdCard,
} from "react-icons/fa";
import { GiTeacher } from "react-icons/gi";
import { MdImportantDevices } from "react-icons/md";

interface Institution {
  name: string;
  address: string;
  nameMunicipality: string;
  urlLogo?: string;
  isMainSchool: boolean;
  regional?: string;
  session?: string;
  phone?: string;
  mobile?: string;
  website?: string;
  nameRector?: string;
  nameCordinator?: string;
  nameSecretary?: string;
  academicResolution?: string;
  educationalRegistry?: string;
}

interface Props {
  institution: Institution;
  setIsEditing: (value: boolean) => void;
}

export const InstitutionCard = ({
  institution,
  setIsEditing,
}: Props) => {
  return (
    <div className="bg-white rounded-2xl shadow-xl overflow-hidden">
      {/* Sección de encabezado */}
      <div className="bg-gradient-to-r from-gray-800 to-gray-700 p-8 text-white">
        <div className="flex flex-col md:flex-row justify-between items-start md:items-center">
          <div className="mb-6 md:mb-0">
            <h1 className="text-3xl font-bold flex items-center">
              <FaSchool className="mr-3" />
              {institution.name}
            </h1>
            <p className="flex items-center mt-3 text-blue-100">
              <FaMapMarkerAlt className="mr-2" />
              {institution.address}, {institution.nameMunicipality}
            </p>
          </div>
          <button
            onClick={() => setIsEditing(true)}
            className="bg-white text-blue-600 hover:bg-blue-50 px-6 py-3 rounded-lg font-medium flex items-center transition-all shadow-md hover:shadow-lg"
          >
            <FaEdit className="mr-2" />
            Editar Institución
          </button>
        </div>
      </div>

      {/* Contenido principal */}
      <div className="p-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Columna izquierda - Logo e información básica */}
          <div className="lg:col-span-1">
            <div className="bg-gray-50 rounded-xl p-6 shadow-inner">
              <div className="flex justify-center mb-6">
                {institution.urlLogo ? (
                  <img
                    src={institution.urlLogo}
                    alt="Logo de la institución"
                    className="w-64 h-64 object-contain rounded-lg border-4 border-white shadow-md"
                  />
                ) : (
                  <div className="w-64 h-64 bg-white rounded-lg flex items-center justify-center shadow-md border-4 border-gray-100">
                    <FaSchool size={80} className="text-gray-300" />
                  </div>
                )}
              </div>

              <div className="space-y-4">
                <div className="bg-white p-4 rounded-lg shadow-sm">
                  <h3 className="font-semibold text-gray-700 mb-3 flex items-center">
                    <FaInfoCircle className="mr-2 text-blue-500" />
                    Información rápida
                  </h3>
                  <div className="space-y-2">
                    <p className="flex items-center text-gray-600">
                      <FaUniversity className="mr-3 text-gray-400" />
                      <span className="font-medium">Tipo:</span>
                      <span className="ml-2">
                        {institution.isMainSchool
                          ? "Escuela Principal"
                          : "Sucursal"}
                      </span>
                    </p>
                    <p className="flex items-center text-gray-600">
                      <FaRegBuilding className="mr-3 text-gray-400" />
                      <span className="font-medium">Regional:</span>
                      <span className="ml-2">
                        {institution.regional || "No especificado"}
                      </span>
                    </p>
                    <p className="flex items-center text-gray-600">
                      <FaClipboardCheck className="mr-3 text-gray-400" />
                      <span className="font-medium">Sesión:</span>
                      <span className="ml-2">
                        {institution.session || "No especificado"}
                      </span>
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Columna derecha - Información detallada */}
          <div className="lg:col-span-2">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {/* Información de contacto */}
              <div className="bg-gray-50 rounded-xl p-6 shadow-inner">
                <h3 className="text-xl font-semibold mb-4 text-gray-700 border-b pb-3 flex items-center">
                  <FaPhone className="mr-3 text-blue-500" />
                  Información de contacto
                </h3>
                <div className="space-y-4">
                  <div className="flex items-start">
                    <div className="bg-blue-100 p-2 rounded-full mr-3">
                      <FaPhone className="text-blue-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">Teléfono</p>
                      <p className="text-gray-600">
                        {institution.phone || "No proporcionado"}
                      </p>
                    </div>
                  </div>
                  <div className="flex items-start">
                    <div className="bg-blue-100 p-2 rounded-full mr-3">
                      <FaMobileAlt className="text-blue-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">Móvil</p>
                      <p className="text-gray-600">
                        {institution.mobile || "No proporcionado"}
                      </p>
                    </div>
                  </div>
                  <div className="flex items-start">
                    <div className="bg-blue-100 p-2 rounded-full mr-3">
                      <FaGlobe className="text-blue-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">Sitio web</p>
                      {institution.website ? (
                        <a
                          href={
                            institution.website.startsWith("http")
                              ? institution.website
                              : `https://${institution.website}`
                          }
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-blue-600 hover:underline"
                        >
                          {institution.website}
                        </a>
                      ) : (
                        <p className="text-gray-600">Sin sitio web</p>
                      )}
                    </div>
                  </div>
                </div>
              </div>

              {/* Personal administrativo */}
              <div className="bg-gray-50 rounded-xl p-6 shadow-inner">
                <h3 className="text-xl font-semibold mb-4 text-gray-700 border-b pb-3 flex items-center">
                  <FaUserTie className="mr-3 text-blue-500" />
                  Personal administrativo
                </h3>
                <div className="space-y-4">
                  <div className="flex items-start">
                    <div className="bg-purple-100 p-2 rounded-full mr-3">
                      <GiTeacher className="text-purple-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">Rector</p>
                      <p className="text-gray-600">
                        {institution.nameRector || "No asignado"}
                      </p>
                    </div>
                  </div>
                  <div className="flex items-start">
                    <div className="bg-purple-100 p-2 rounded-full mr-3">
                      <FaChalkboardTeacher className="text-purple-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">Coordinador</p>
                      <p className="text-gray-600">
                        {institution.nameCordinator || "No asignado"}
                      </p>
                    </div>
                  </div>
                  <div className="flex items-start">
                    <div className="bg-purple-100 p-2 rounded-full mr-3">
                      <FaUserTie className="text-purple-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">Secretario</p>
                      <p className="text-gray-600">
                        {institution.nameSecretary || "No asignado"}
                      </p>
                    </div>
                  </div>
                </div>
              </div>

              {/* Información legal */}
              <div className="md:col-span-2 bg-gray-50 rounded-xl p-6 shadow-inner">
                <h3 className="text-xl font-semibold mb-4 text-gray-700 border-b pb-3 flex items-center">
                  <FaIdCard className="mr-3 text-blue-500" />
                  Información legal
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div className="flex items-start">
                    <div className="bg-green-100 p-2 rounded-full mr-3">
                      <FaClipboardCheck className="text-green-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">
                        Resolución académica
                      </p>
                      <p className="text-gray-600">
                        {institution.academicResolution || "No proporcionado"}
                      </p>
                    </div>
                  </div>
                  <div className="flex items-start">
                    <div className="bg-green-100 p-2 rounded-full mr-3">
                      <MdImportantDevices className="text-green-600" />
                    </div>
                    <div>
                      <p className="font-medium text-gray-700">
                        Registro educativo
                      </p>
                      <p className="text-gray-600">
                        {institution.educationalRegistry || "No proporcionado"}
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
