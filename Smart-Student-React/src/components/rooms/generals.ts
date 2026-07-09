
export const getItemsPerPage = (): number => {
    if (typeof window !== "undefined") {
        const isMobile = window.innerWidth <= 768;
        return isMobile ? 5 : 6;
    }
    return 6;
};


export const itemsPerPage = getItemsPerPage();
