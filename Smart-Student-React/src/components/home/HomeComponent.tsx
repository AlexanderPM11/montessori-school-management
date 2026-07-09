import { useEffect, useState } from "react";
import { FaEdit, FaSchool } from "react-icons/fa";
import { useInstituCentersStore } from "../../hooks/store/InstituCenter/InstituCenter.store";
import { useGeneralStore } from "../../Formik/Users/user";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { InstitutionCard } from "./InstitutionCard";
import { EditarInstitucionCard } from "./EditarInstitucionCard";

export const HomeComponent = () => {
  const [isEditing, setIsEditing] = useState(false);

  const institution = useInstituCentersStore((state) => state.instituCenters);
  const isLoading = useInstituCentersStore((state) => state.loading);

  const GetGeneralsUserRoles = useGeneralStore(
    (state) => state.GetGeneralsUserRoles
  );
  const getGetInstituCenter = useInstituCentersStore(
    (state) => state.getGetInstituCenter
  );
  const GetProvices = useGeneralStore((state) => state.getProvices);

  // Fetch inicial
  useEffect(() => {
    const fetchData = async () => {
      try {
        await Promise.all([
          GetGeneralsUserRoles(),
          GetProvices(),
          getGetInstituCenter(),
        ]);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // Manejo de loader
  useEffect(() => {
    if (isLoading) {
      showCustomLoading();
    } else {
      closeCustomLoading();
    }
  }, [isLoading]);

  if (isLoading) {
    return null;
  }

  // Cuando no hay institución
  if (!institution || Object.keys(institution).length === 0) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gradient-to-br from-blue-50 to-white">
        {isEditing ? (
          <EditarInstitucionCard onCancel={() => setIsEditing(false)} />
        ) : (
          <div className="text-center p-8 bg-white rounded-xl shadow-lg max-w-md">
            <FaSchool className="mx-auto text-5xl text-blue-500 mb-4" />
            <h2 className="text-2xl font-bold text-gray-800 mb-2">
              No hay datos institucionales
            </h2>
            <p className="text-gray-600 mb-6">
              Actualmente no hay información institucional disponible.
            </p>
            <button
              onClick={() => setIsEditing(true)}
              className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg font-medium flex items-center mx-auto transition-all"
            >
              <FaEdit className="mr-2" />
              Create New Institution
            </button>
          </div>
        )}
      </div>
    );
  }

  // Cuando ya hay institución registrada
  return (
    <div className="space-y-6 bg-gradient-to-br to-white max-w-[1200px]">
      <h1 className="text-2xl md:text-3xl font-bold text-gray-800 mb-6">
        Centro Educativo
      </h1>
      <div className="max-w-7xl mx-auto">
        {isEditing ? (
          <EditarInstitucionCard onCancel={() => setIsEditing(false)} />
        ) : (
          <InstitutionCard
            institution={institution}
            setIsEditing={setIsEditing}
          />
        )}
      </div>
    </div>
  );
};
