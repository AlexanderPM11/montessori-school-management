import { create, StateCreator } from "zustand";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { EducationalInstitutionViewModel } from "../../../interfaces/IntiituCenter/EducationalInstitutionViewModel";
import InstituCenterService from "../../../services/InstituCenter/InstituCenterService";

interface InstituCenterstate {
    // Properties
    loading: boolean;
    getInstituCenter: boolean;
    instituCenters: EducationalInstitutionViewModel;

    // Actions
    getGetInstituCenter: () => Promise<
        ApiResponse<EducationalInstitutionViewModel>
    >;
    onCreateOrUpdate: (
        InstituCenter: EducationalInstitutionViewModel
    ) => Promise<ApiResponse<number>>;
}

const creatorInstituCenter: StateCreator<InstituCenterstate> = (set) => ({
    loading: true,
    getInstituCenter: false,
    instituCenters: {} as EducationalInstitutionViewModel,

    getGetInstituCenter: async (): Promise<
        ApiResponse<EducationalInstitutionViewModel>
    > => {
        try {
            set(() => ({ loading: true }));

            const data = await InstituCenterService.GetInstituCenter();

            set((state) => ({
                ...state,
                loading: false,
                getInstituCenter: true,
                instituCenters: data.data,
            }));

            return data;
        } catch (error) {
            console.error("Error during getGetInstituCenter:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<EducationalInstitutionViewModel>;
        }
    },

    onCreateOrUpdate: async (
        InstituCenter: EducationalInstitutionViewModel
    ): Promise<ApiResponse<number>> => {
        try {
            const response = await InstituCenterService.CreateOrUpdate(InstituCenter);

            if (response.result) {
                // Aquí actualizas directamente el estado con los datos enviados
                set((state) => ({
                    ...state,
                    instituCenters: InstituCenter,
                }));
            }

            return response;
        } catch (error) {
            console.error("Error during onCreateOrUpdate:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    }

});

export const useInstituCentersStore =
    create<InstituCenterstate>()(creatorInstituCenter);
