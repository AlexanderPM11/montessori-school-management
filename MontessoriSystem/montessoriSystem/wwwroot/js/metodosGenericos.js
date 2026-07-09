
function showGlobalLoadingIndicator() {
    document.getElementById('loading').style.display = 'block';
}

function hideGlobalLoadingIndicator() {
    document.getElementById('loading').style.display = 'none';
}
function convertirFecha(cadenaFecha) {
    // Dividir la cadena de fecha en día, mes y año
    var partesFecha = cadenaFecha.split(' ');
    var dia = parseInt(partesFecha[0]);
    var mes = partesFecha[1];
    var año = parseInt(partesFecha[2]);

    // Convertir el mes de texto a número
    var meses = {
        'Enero': 0,
        'Febrero': 1,
        'Marzo': 2,
        'Abril': 3,
        'Mayo': 4,
        'Junio': 5,
        'Julio': 6,
        'Agosto': 7,
        'Septiembre': 8,
        'Octubre': 9,
        'Noviembre': 10,
        'Diciembre': 11
    };
    var mesNumero = meses[mes];

    // Crear un objeto de fecha
    var fecha = new Date(año, mesNumero, dia);
    return fecha;
}

function parseDateLongToShort(fechaCompleta) {
    // Dividir la cadena de fecha en partes usando el espacio como separador
    var partes = fechaCompleta.split(' ');
    // Tomar solo la primera parte que contiene la fecha
    var fecha = partes[0];
    return fecha;
}

function convertTo24HourFormat(hora12h) {
    var partesHora = hora12h.split(':');
    var hora = parseInt(partesHora[0], 10);
    var minutos = parseInt(partesHora[1], 10);

    if (hora === 12 && hora12h.toLowerCase().indexOf('am') !== -1) {
        hora = 0; // Si son las 12 AM, convertir a 0
    } else if (hora !== 12 && hora12h.toLowerCase().indexOf('pm') !== -1) {
        hora += 12; // Si es PM (excepto las 12 PM), sumar 12 horas
    }

    // Formatear la hora y los minutos en formato de 24 horas
    var hora24h = hora.toString().padStart(2, '0') + ':' + minutos.toString().padStart(2, '0');
    return hora24h;
}
function convertTo12HourFormat(hora24h) {
    var partesHora = hora24h.split(':');
    var hora = parseInt(partesHora[0], 10);
    var minutos = partesHora[1];
    var ampm = hora >= 12 ? 'PM' : 'AM';

    // Convertir horas de 24 a 12 horas
    if (hora === 0) {
        hora = 12;
    } else if (hora > 12) {
        hora -= 12;
    }

    // Formatear la hora y los minutos en formato de 12 horas
    var hora12h = hora.toString().padStart(2, '0') + ':' + minutos + ' ' + ampm;
    return hora12h;
}


function obtenerColorAleatorio() {
    return coloresPasteles[Math.floor(Math.random() * coloresPasteles.length)];
}

var coloresPasteles = [
    '#a3e4d7', // Azul verdoso claro
    '#82e0aa', // Verde menta claro
    '#7dcea0', // Verde esmeralda pálido
    '#6ab04c', // Verde primavera claro
    '#48c9b0', // Azul verdoso pastel
    '#45b39d', // Verde esmeralda pastel
    '#52be80', // Verde manzana pastel
    '#2e86c1', // Azul pastel
    '#16a085', // Verde turquesa pastel
    '#1abc9c', // Verde menta pastel
    '#1dd1a1', // Verde agua pastel
    '#16a085' // Verde turquesa claro
];
function formatDateString(inputDateString) {
    // Crear un objeto de fecha desde la cadena de fecha de entrada
    var date = new Date(inputDateString);

    // Array de nombres de meses abreviados
    var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

    // Obtener el mes abreviado según el índice del mes
    var monthIndex = date.getMonth();
    var monthName = monthNames[monthIndex];

    // Obtener el día y el año
    var day = date.getDate();
    var year = date.getFullYear();

    // Formatear la fecha en el formato deseado
    var formattedDate = day + " " + monthName +  " " + year;

    return formattedDate;
}
function GetCurrentAge(inputDateString) {
    var birthDate = new Date(inputDateString);
    var currentDate = new Date();

    var age = currentDate.getFullYear() - birthDate.getFullYear();
    var currentMonth = currentDate.getMonth() + 1;
    var birthMonth = birthDate.getMonth() + 1;

    // Check if the birthday month has not been reached in the current year
    if (currentMonth < birthMonth || (currentMonth === birthMonth && currentDate.getDate() < birthDate.getDate())) {
        age--;
    }

    return age;
}

//function formatterEmail(idInput) {
    

//    const input = document.getElementById(idInput);
//    input.addEventListener('input', function () {

//        let value = this.value.replace(/\D/g, '');
//        if (value.length > 3) {
//            value = value.substring(0, 3) + '-' + value.substring(3);
//        }
//        if (value.length > 6) {
//            value = value.substring(0, 7) + '-' + value.substring(7, 11);
//        }
//        this.value = value;
//    });
//}
function formatterTel(idInput) {
    

    const input = document.getElementById(idInput);
    input.addEventListener('input', function () {

        let value = this.value.replace(/\D/g, '');
        if (value.length > 3) {
            value = value.substring(0, 3) + '-' + value.substring(3);
        }
        if (value.length > 6) {
            value = value.substring(0, 7) + '-' + value.substring(7, 11);
        }
        this.value = value;
    });
}
function formatterIdenticationId(idInput) {

    const input = document.getElementById(idInput);
    input.addEventListener('input', function () {
        let value = $(this).val().replace(/\D/g, '');
        if (value.length > 3) {
            value = value.substring(0, 3) + '-' + value.substring(3);
        }
        if (value.length > 11) {
            value = value.substring(0, 11) + '-' + value.substring(11, 12);
        }
        this.value = value;
    });    
}


function restrictInputToNumbers(inputId) {
    const input = document.getElementById(inputId);
    input.addEventListener("input", function () {
        const inputValue = this.value;
        this.value = inputValue.replace(/[^\d]/g, ''); // Eliminar caracteres no numéricos
    });
}