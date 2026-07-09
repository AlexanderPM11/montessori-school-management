import { create, StateCreator } from 'zustand'
import { ApiResponse } from '../../../interfaces/ApiResponse';
import { Adjunto } from '../../../interfaces/Adjunto/Ajunto';
import AdjuntoService from '../../../services/Adjunto/AdjuntoService';


interface AdjuntoState {

    // Properties
    loading: boolean;
    adjuntos: Adjunto[];
    idStudentStore: number;

    // Actions
    getAdjunto: (idStudent: number) => Promise<ApiResponse<Adjunto[]>>;
    onCreateOrUpdate: (adjunto: Adjunto) => Promise<ApiResponse<number>>;
    onDelete: (idAdjunto: number) => Promise<ApiResponse<number>>;

}


const creatorAdjunto: StateCreator<AdjuntoState> = (set, get) => ({
    loading: false,
    adjuntos: [],
    idStudentStore: 0,

    getAdjunto: async (idStudent: number): Promise<ApiResponse<Adjunto[]>> => {
        try {
            const adjuntosStore = get().adjuntos;
            const idStudentStore = get().idStudentStore;

            if (idStudentStore == idStudent) {
                if (idStudentStore == idStudent || !Array.isArray(adjuntosStore) || adjuntosStore.length > 0) {
                    return {} as ApiResponse<Adjunto[]>;
                }
            }

            set(() => ({ loading: true }));
            const dataReponse = await AdjuntoService.GetAdjunto(idStudent);

            const adjuntos = dataReponse.data ?? [];

            set((state) => ({

                ...state,
                adjuntos,
                idStudentStore: idStudent,
                loading: false,
            }));

            return dataReponse;
        } catch (error) {
            console.error("Error during login:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<Adjunto[]>;
        }
    },
    onCreateOrUpdate: async (adjunto: Adjunto): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await AdjuntoService.CreateOrUpdate(adjunto);
            if (data.result) {
                if (adjunto.id === 0) {
                    adjunto.id = data.data;
                }

                set((state) => {

                    if (!Array.isArray(state.adjuntos) || state.adjuntos.length === 0) {
                        state.adjuntos.push(adjunto);
                        return { ...state, loading: false };
                    }
                    const updatedAdjuntos = [...state.adjuntos];
                    const userIndex = updatedAdjuntos.findIndex((u) => u.id === adjunto.id);
                    if (userIndex !== -1) {
                        // Si el usuario existe, actualizarlo
                        updatedAdjuntos[userIndex] = adjunto;
                    } else {
                        // Si no existe, agregarlo
                        updatedAdjuntos.push(adjunto);
                    }
                    return {
                        ...state,
                        adjuntos: updatedAdjuntos,
                        loading: false,
                    };
                });
            } else {
                set(() => ({ loading: false }));
            }




            return data;

        } catch (error) {
            console.error("Error during onCreateOrUpdate:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
    onDelete: async (idAdjunto: number): Promise<ApiResponse<number>> => {
        try {
            set(() => ({ loading: true }));

            const data = await AdjuntoService.Delete(idAdjunto);

            // Si la eliminación en el servidor fue exitosa, eliminar localmente
            if (data.result) {
                set((state) => {
                    const updatedAdjuntos = state.adjuntos.filter(
                        (adjunto) => adjunto.id !== idAdjunto
                    );
                    return {
                        ...state,
                        adjuntos: updatedAdjuntos,
                        loading: false,
                    };
                });
            } else {
                set(() => ({ loading: false }));
            }

            return data;

        } catch (error) {
            console.error("Error during onDelete:", error);
            set(() => ({ loading: false }));
            return {} as ApiResponse<number>;
        }
    },
});



export const useAdjuntoStore = create<AdjuntoState>()(
    creatorAdjunto
);
