using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Application.ViewModels.Suject;


namespace MontessoriSystem.Infrastructure.Persistence.Seed
{
    public static class AchievementIndicatorsSeed
    {
        public static async Task SetAchievementIndicators(IAchievementIndicatorsService indicatorsService)
        {
            var gradeData = new Dictionary<string, string>
            {
                #region Párvulo I

                { "Expresa sus necesidades, sentimientos llorando, moviendo su cuerpo o vocalizando y gestos de satisfacción al reconocer personas familiares.", "Párvulo I" },
                { "Muestra agrado ante las demostraciones de afecto por medio al contacto corporal, visual, por medio de conversaciones, arrullos y nanas.", "Párvulo I" },
                { "Imita las reacciones y movimientos que ve en las interacciones con sus cuidadores. ", "Párvulo I" },
                { "Explora su entorno cercano con apoyo del adulto o por sí solo.", "Párvulo I" },
                { "Reacciona al escuchar su nombre.", "Párvulo I" },
                { "Presta atención al lenguaje reaccionando a la interacción comunicativa de personas cercanas y las canciones de cuna o arrullos.", "Párvulo I" },
                { "Crea los significados a partir de la narración que hace el adulto sobre gestos, vocalizaciones, las acciones y emociones del momento.", "Párvulo I" },
                { "Expresa sus necesidades por medio de gestos globales e indiferenciados, contacto visual, gritos, llantos y movimientos involuntarios.", "Párvulo I" },
                { "Observa con interés los objetos, imágenes de libros ilustrados.", "Párvulo I" },
                { "Sostiene la cabeza y busca con la misma los sonidos que escucha del medio.", "Párvulo I" },
                { "Percibe las partes de su cuerpo mediante masajes y movimientos o juegos corporales que le realiza el adulto.", "Párvulo I" },
                { "Juega con manos y pies usando juguetes, utiliza sus dedos para agarra objetos pequeños, lo entra y lo saca de una caja con apoyo interactuando en su medio.", "Párvulo I" },
                { "Manifiesta sus emociones espontáneamente con movimientos del cuerpo y por medio de gestos y llantos", "Párvulo I" },
                { "Realiza movimientos con el cuerpo: expresiones faciales, gestos y llantos ante situaciones en el entorno y acciones que percibe del adulto.", "Párvulo I" },
                { "Empuja su cuerpo con las piernas sobre una superficie plana.", "Párvulo I" },
                { "Gira su cuerpo con su abdomen a 360 grados empujándose con los pies.", "Párvulo I" },
                { "Arrastra su cuerpo colocándolo en patrón cruzado, mano izquierda y pie derecho, mano derecha y pie izquierdo.", "Párvulo I" },
                { "Alcanza objetos con las manos en posición boca abajo e impulsándose con los pies.", "Párvulo I" },
                { "Balancea su cuerpo de derecha a izquierda.", "Párvulo I" },
                { "Desplaza su cuerpo gateando y arrastrándose sobre superficies planas.", "Párvulo I" },
                { "Manifiesta alegría al observarse cuando ve su imagen en el espejo con la asistencia de un adulto.", "Párvulo I" },
                { "Juega con algunas partes de su cuerpo como son las manos y los pies.", "Párvulo I" },
                { "Muestra placer y alegría al jugar tocándose sus manos, pies y los dedos.", "Párvulo I" },
                
                //45 días a 1 año

                { "Expresa sentimientos, necesidades e intereses con vocalizaciones, gestos, caricias y palabras..", "Párvulo I" },
                { "Reacciona ante las demostraciones de afecto de las personas familiares y extrañas.", "Párvulo I" },
                { "Demuestra apego hacia las personas de su afecto y se separa por corto tiempo.", "Párvulo I" },
                { "Participa en actividades de su higiene y alimentación, de acuerdo con su desarrollo motor.", "Párvulo I" },
                { "Reconoce su imagen y de personas cercanas en diferentes representaciones.", "Párvulo I" },
                { "Responde cuando le llaman por su nombre e intenta decirlo.", "Párvulo I" },
                { "Reconoce por su nombre a miembros de su familia y personas significativas.", "Párvulo I" },
                { "Explora por sí solo su entorno cercano y reconoce objetos.", "Párvulo I" },
                { "Imita normas sociales en sus interacciones.", "Párvulo I" },
                { "Imita gestos y expresiones al manifestar sus necesidades a sus pares y personas adultas.", "Párvulo I" },
                { "Usa espontáneamente gestos, expresiones al manifestar sus necesidades, con sus pares y personas adultas.", "Párvulo I" },
                { "Comprende las informaciones emitidas durante la conversación con pares y otras personas.", "Párvulo I" },
                { "Usa palabra- frase, gestos y movimientos corporales unidas a las acciones cotidianas al expresar sus intereses, emociones y necesidades.", "Párvulo I" },
                { "Presta atención, aprecia las imágenes de libros ilustrados y comprende nombres, elementos, personajes y algunas de sus características.", "Párvulo I" },
                { "Interactúa y disfruta las situaciones comunicativas que integran libros de imágenes y las expresiones literarias acompañados de movimientos.", "Párvulo I" },
                { "Se pone de pie con apoyos de un adulto para lograrlo, iniciando el proceso del caminar.", "Párvulo I" },
                { "Mantiene el equilibrio en cambios de posición de sentado a parado.", "Párvulo I" },
                { "Toca con sus manos algunas partes de su cuerpo imitando acciones del adulto.", "Párvulo I" },
                { "Juega con sus manos y pies al interactuar con juguetes y objetos de uso cotidiano.", "Párvulo I" },
                { "Imita movimientos sencillos de las manos, los brazos y la cabeza del adulto para expresarse con su cuerpo.", "Párvulo I" },
                { "Realiza movimientos del cuerpo, gestos y llantos ante situaciones del entorno y acciones que percibe del adulto.", "Párvulo I" },
                { "Manifiesta sus emociones espontáneamente con movimientos del cuerpo y por medio de gestos y llantos.", "Párvulo I" },
                { "Desplaza su cuerpo gateando y arrastrándose sobre superficies planas..", "Párvulo I" },
                { "Gira su cuerpo rodando en diferentes direcciones.", "Párvulo I" },
                { "Arrastra su cuerpo reptando en dirección frontal.", "Párvulo I" },
                { "Alcanza objetos con las manos en posición boca abajo e impulsándose con los pies", "Párvulo I" },
                { "Balancea su cuerpo de derecha a izquierda", "Párvulo I" },
                { "Aprieta y suelta objetos blandos por indicación e imitación del adulto.", "Párvulo I" },
                { "Interactúa con juguetes y objetos de uso cotidiano.", "Párvulo I" },
                { "Entra y saca objetos de una caja sin asistencia y con la indicación de un adulto.", "Párvulo I" },
                { "Juega con algunas partes de su cuerpo como son las manos y los pies", "Párvulo I" },
                { "Muestra placer y alegría al jugar tocándose sus manos, pies y los dedos", "Párvulo I" },

                #endregion

                #region Párvulo II
                
                { "Reconoce objetos y espacios habituales.", "Párvulo II" },
                { "Usa normas sociales en sus interacciones cuando se le solicita.", "Párvulo II" },
                { "Expresa sentimientos y afectos hacia otros.","Párvulo II" },
                { "Juega mientras explora el ambiente que le rodea con cierta independencia y seguridad.", "Párvulo II" },
                { "Decide qué actividad le gusta o no realizar.", "Párvulo II" },
                { "Se identifica a sí mismo, a sus pares y personas cercanas en diferentes representaciones.", "Párvulo II" },
                { "Responde y expresa su nombre.", "Párvulo II" },
                { "Tolera en sus interacciones si otras personas utilizan juguetes u objetos comunes.", "Párvulo II" },

                { "Reconoce objetos, personas, animales del entorno cercano nombrados por otra persona en situaciones comunicativas.", "Párvulo II" },
                { " Sigue instrucciones asociados a actividades de rutina expresadas en oraciones simples y palabras conocidas.","Párvulo II" },
                { "Responde preguntas simples.", "Párvulo II" },
                { "Imita sonidos y movimientos expresados en cuentos cortos/ repetitivo, poesías rimadas, canciones. ", "Párvulo II" },
                { "Expresa sus necesidades fisiológicas, socioafectivas y/o de juego acompañando su acción " +
                "con palabras, primeras frases, gestos y movimientos voluntarios.", "Párvulo II" },
                { "Nombra objetos y acciones mientras juega, conversa y explora su entorno cercano.", "Párvulo II" },
                { "Elige e imita la acción de leer al explorar libros ilustrados de su interés.", "Párvulo II" },
                { "Formula pregunta al explorar libros ilustrados.","Párvulo II" },

                { "Controla de manera progresiva su cuerpo al colocarse de pie sin apoyo.", "Párvulo II" },
                { "Manipula con sus manos y pies diferentes materiales y juguetes para producir sonidos libremente a partir de movimientos con su cuerpo.", "Párvulo II" },
                { "Usa sus manos pelotas pequeñas y medianas y otros objetos, intentando orientarlos hacia un punto.", "Párvulo II" },
                { "Realiza movimientos en los que coordina ojo-pie y ojo-mano como patear pelotas sin orientación específica o atrapar una pelota pequeña.", "Párvulo II" },
                { "Expresa ideas y sentimientos libremente de forma gráfica de manera no convencional.", "Párvulo II" },
                { "Realiza juegos y ejercicio de arrastres, de empuje y golpeo de objetos usando sus manos y pies.", "Párvulo II" },

                #endregion

                #region Párvulo III

                { "Elige y completa una actividad simple.", "Párvulo III" },
                { "Asume responsabilidades de acuerdo con su edad cuando se le pide.", "Párvulo III" },
                { "Da las gracias, pide por favor, saluda y se despide al salir.", "Párvulo III" },
                { "Intenta el cumplimiento de acuerdos de pedir la palabra y espera su turno para hablar o realizar actividades rutinarias", "Párvulo III" },
                { "Disfruta del juego e interactúa con otras personas motivado por el adulto.", "Párvulo III" },
                { "Interactúa con otros niños y niñas expresando sus propios intereses de acuerdo a sus posibilidades.", "Párvulo III" },
                { "Interactúa siguiendo las orientaciones y aceptando que otros utilicen los objetos y juguetes comunes.", "Párvulo III" },
                { "Utiliza gestos y palabras para manifestar sus emociones.", "Párvulo III" },
                { "Responde a su nombre y apellidos.", "Párvulo III" },
                { "Dice su nombre, edad y sexo.", "Párvulo III" },
                { "Conoce el nombre de personas significativas de su entorno.", "Párvulo III" },
                { "Construye torres con mayor precisión.", "Párvulo III" },

                { "Practica normas de autocuidado con ayuda.", "Párvulo III" },
                { "Nombra y reconoce algunas de las funciones de las partes externas de su cuerpo.", "Párvulo III" },
                { "Evita participar en situaciones que le provocan inseguridad buscando al adulto de su afecto cuando se le presentan.", "Párvulo III" },
                { "Sigue algunas advertencias de seguridad ante objetos y el espacio cercano.", "Párvulo III" },
                { "Colabora en el cuidado de su cuerpo, objetos y espacios que usa.", "Párvulo III" },
                { "Cede ante la solicitud de devolución de objetos o juguetes comunes.", "Párvulo III" },
                { "Se aleja del adulto de su afecto interactuando con otras personas en situaciones que le producen bienestar y seguridad.", "Párvulo III" },
                { "Realiza juegos en los que identifica y señala algunas partes de su cuerpo reaccionando a la pregunta de un adulto.", "Párvulo III" },
                { "Interactúa con elementos y objetos de su entorno inmediato, en relación con su tamaño, posición, distancia, espacio, tiempo, peso, textura, temperatura y forma.", "Párvulo III" },
                { "Relaciona las características de su propio cuerpo con la de sus pares.", "Párvulo III" },
                { "Realiza juegos y actividades motrices con tiempo medido.", "Párvulo III" },
                { "Realiza algunas acciones de vestirse como sacarse los zapatos y colabora con el adulto para que lo vista.", "Párvulo III" },
                { "Participa en juegos y otras acciones motrices para expresar ideas, sentimiento y emociones, al interactuar con su entorno físico y sus familiares más cercanos.", "Párvulo III" },
                { "Se desplaza caminando, corriendo, y saltando en superficies planas en diversas direcciones, a distancias cortas, con o sin obstáculos con estabilidad, equilibrio y confianza.", "Párvulo III" },
                { "Trepa sobre superficies inclinadas con progresiva confianza.", "Párvulo III" },
                { "Mantiene el equilibrio en la ejecución de desplazamientos como caminar por una línea recta y curvas trazadas en el piso y trepar pequeñas alturas.", "Párvulo III" },
                { "Participa en acciones cotidianas de lavado de manos, bañarse y cepillarse con apoyo del adulto.", "Párvulo III" },

                { "Participa en juegos en los que se organiza en filas, hileras, círculos y semicírculos junto a los compañeros.", "Párvulo III" },
                { "Desarrolla creativamente su imaginación mediante gestos, mímicas, movimientos rítmicos (danzas y formas jugadas), dramatización e imitación de situaciones y movimientos de su entorno", "Párvulo III" },
                { "Agarra con las manos objetos grandes y pequeños como medio de exploración de su entorno.", "Párvulo III" },
                { "Lanza pelotas pequeñas, medianas y otros objetos, intentando orientarlos hacia un punto.", "Párvulo III" },
                { "Realiza juegos y ejercicios de  arrastres, de empuje y golpeo de objetos blandos usando sus manos y pies.", "Párvulo III" },
                { "Comprende las informaciones y/o mensajes de los adultos y sus pares.", "Párvulo III" },
                { "Realiza preguntas para aclarar los significados relacionados con experiencias y elementos de vida cotidiana.", "Párvulo III" },
                { "Realiza acciones o juegos relacionando experiencias o vivencias cotidianas.", "Párvulo III" },
                { "Reconoce sonidos, mímicas, gestos y emociones relacionados con su medio social y natural.", "Párvulo III" },
                { "Realiza las instrucciones simples relacionadas con vida cotidiana.", "Párvulo III" },
                { "Escucha con atención y disfruta textos literarios, narrativos y poéticos tanto tradicionales como contemporáneos.", "Párvulo III" },
                { "Dramatiza acciones relacionadas con el contenido de textos narrativos y poéticos.", "Párvulo III" },
                { "Participa en situaciones comunicativas en pequeños grupos y con todo el grupo.", "Párvulo III" },
                { "Emplea el lenguaje para expresar necesidades, sentimientos y emociones.", "Párvulo III" },
                { "Muestra respeto a las normas de conversación.", "Párvulo III" },
                { "Lee no convencionalmente libros ilustrados con textos poéticos y narrativos.", "Párvulo III" },
                { "Pregunta sobre el contenido de libros ilustrados.", "Párvulo III" },
                { "Realiza  narraciones cortas sobre el contenido de textos literarios sin seguir el orden lógico", "Párvulo III" },

                #endregion
                
                #region Pre-Kinder

                { "Expresa sus sentimientos, emociones e ideas de forma gráfica o escrita, al escuchar narraciones, mensajes, cuentos, canciones, poesías leídas por otras personas, de manera no convencional.", "Pre-Kinder" },
                { "Sigue una secuencia en el uso de algunos textos orales, en situaciones cotidianas de comunicación al expresar sentimientos, gestos, emociones y movimientos corporales.", "Pre-Kinder" },
                { "Usa diversas formas de expresión con intención de comunicar sus ideas, opiniones, emociones, sentimientos y experiencias.", "Pre-Kinder" },
                { "Realiza producciones plásticas bidimensionales (dibujo) al representar ideas reales o imaginarias", "Pre-Kinder" },
                { "Maneja títeres de dedos y manos junto a compañeros y compañeras, cantando o contando historias.", "Pre-Kinder" },
                { "Conversa acerca de las actuaciones de los personajes en dramatizaciones y obras de títeres que observa.", "Pre-Kinder" },
                { "Realiza movimientos en diferentes direcciones y velocidades, de forma libre y siguiendo un ritmo.", "Pre-Kinder" },
                { "Realiza juegos de desplazamiento en el espacio, mostrando progresión en la estabilidad y equilibrio de su cuerpo según indicaciones derecha- izquierda.", "Pre-Kinder" },
                { "Explora y manipula objetos y los incorpora al juego o improvisaciones lúdicas.", "Pre-Kinder" },
                { "Describe situaciones de su comunidad local a partir de preguntas, con el apoyo de adultos.", "Pre-Kinder" },
                { "Participa en actividades relacionadas con la comunidad local.", "Pre-Kinder" },
                { "Comparte informaciones con ayuda del adulto, sobre temas de interés de su entorno inmediato.", "Pre-Kinder" },
                { "Ordena hechos sencillos de la realidad ocurridos en su entorno inmediato utilizando su imaginación", "Pre-Kinder" },
                { "Predice acontecimientos explicando lo ocurrido de acuerdo a su conocimiento y experiencias previas.", "Pre-Kinder" },
                { "Asume progresivamente las reglas acordadas en algunos juegos", "Pre-Kinder" },
                { "Escucha, textos cortos, asociando imágenes, palabras y frases cortas, mediante la exploración diferentes recursos y medios.", "Pre-Kinder" },
                { "Comunica ideas mediante imágenes, textos funcionales e icónicos en formato digitales e impreso.", "Pre-Kinder" },
                { "Comprende e interpreta textos orales sencillos que observa y escucha, formulando y respondiendo preguntas en la lengua materna del país.", "Pre-Kinder" },
                { "Colabora en la puesta en práctica de la solución acordada de problemas sencillos o situaciones cotidianas que afectan a sí mismo o al grupo.", "Pre-Kinder" },
                { "Responde a preguntas sobre situaciones o actividades que se desarrollan en su contexto inmediato", "Pre-Kinder" },
                { "Comprende, progresivamente, el orden lógico de actividades en una rutina.", "Pre-Kinder" },
                { "Realiza producciones plásticas bidimensionales (dibujo) seleccionando y combinando colores primarios y secundarios al representar ideas reales o imaginarias.", "Pre-Kinder" },
                { "Explora con su cuerpo sonidos y movimientos, de forma libre y siguiendo un ritmo.", "Pre-Kinder" },
                { "Explora las posibilidades sonoras de su voz y de sonidos producidos al percutir, sacudir o frotar instrumentos musicales y objetos o materiales de su entorno.", "Pre-Kinder" },
                { "Experimenta con su cuerpo movimientos de estiramiento, relajación y descanso, al iniciar y al finalizar actividades físicas.", "Pre-Kinder" },
                { "Imita de manera espontánea situaciones reales e imaginarias a partir canciones y juegos de rondas.", "Pre-Kinder" },
                { "Asume progresivamente las reglas acordadas en algunos juegos.", "Pre-Kinder" },
                { "Cuestiona, observa y explora su entorno natural a partir de temas de interés.", "Pre-Kinder" },
                { "Experimenta con su cuerpo movimiento, estiramiento, respiración, relajación y descanso.", "Pre-Kinder" },
                { "Describe algunas características de los seres vivos de su entorno.", "Pre-Kinder" },
                { "Integra de manera espontánea objetos del entorno cercano a sus juegos simulando situaciones reales o imaginarias.", "Pre-Kinder" },
                { "Responde a preguntas sobre situaciones o actividades que se desarrollan en su contexto inmediato.", "Pre-Kinder" },
                { "Expresa sus ideas sobre lo que observa y las actividades cotidianas que realiza.", "Pre-Kinder" },
                { "Produce textos orales y los expone en lenguaje no verbal (gestos, ademanes, postura…) y paraverbal (entonación, ritmo, pausa, intensidad…), de acuerdo a sus necesidades", "Pre-Kinder" },
                { "Escucha su nombre e intenta escribirlo de forma no convencional o progresivamente convencional.", "Pre-Kinder" },
                { "Utiliza la narración en las formas convencionales de lectura con ayuda del adulto.", "Pre-Kinder" },
                { "Expresa las acciones y actitudes que le producen bienestar o no a su persona y a los demás.", "Pre-Kinder" },
                { "Participa en el cuidado del entorno escolar.", "Pre-Kinder" },
                { "Demuestra afecto a través de sus gestos, palabras y comportamientos.", "Pre-Kinder" },
                { "Entona canciones infantiles y las acompaña con instrumentos de percusión menor.", "Pre-Kinder" },
                { "Reproduce secuencia rítmica y melódica con su cuerpo e instrumentos musicales.", "Pre-Kinder" },
                { "Participa y colabora en actividades festivas culturales y folklóricas relacionadas con la comunidad local.", "Pre-Kinder" },
                { "Integra de manera espontánea objetos del entorno cercano a sus juegos simulando situaciones reales o imaginarias", "Pre-Kinder" },
                { "Participa en actividades de higiene y cuidado personal, con apoyo del adulto", "Pre-Kinder" },
                { "Reproduce secuencia rítmica y melódica con su cuerpo e instrumentos musicales", "Pre-Kinder" },
                { "Dialoga sobre el cuidado y respeto a los seres vivos y su entorno.", "Pre-Kinder" },
                { "Participan en el cuidado del entorno escolar.", "Pre-Kinder" },
                { "Colabora en la reducción de la producción de desecho y en su depósito adecuado; así como en acciones cotidianas de uso adecuado de agua y fuente de energía", "Pre-Kinder" },
                { "Expresa a los demás sus acuerdos o desacuerdos ante situaciones o hechos cotidianos.", "Pre-Kinder" },
                { "Participa en la búsqueda y selección de alternativas al solucionar situaciones cotidianas.", "Pre-Kinder" },
                { "Colabora en la puesta en práctica de la solución acordada de problemas sencillos o situaciones cotidianas que afectan a sí mismo o al grupo", "Pre-Kinder" },
                { "Respeta las reglas de convivencia y comunicación, esperando su turno para formular preguntas sencillas de su interés.", "Pre-Kinder" },
                { "Identifica problemas del entorno y expresa posibles soluciones mediante un tipo de texto oral o manera no convencional.", "Pre-Kinder" },
                { "Explora textos orales y escritos, sencillos, exponiendo los resultados mediante imágenes, textos orales, utilizando recursos y diversos medios.", "Pre-Kinder" },
                { "Expresa los valores que escucha e imita, lo escribe o reproduce en el texto oral o de forma no convencional.", "Pre-Kinder" },

                #endregion

                #region Kinder

                { "Comunica sus ideas, pensamientos, emociones y experiencias con la finalidad de que los demás puedan entender lo que pretende comunicar.", "Kinder" },
                { "Utiliza técnicas y materiales en sus producciones para expresar sus emociones, sentimientos, ideas y experiencias.", "Kinder" },
                { "Escucha y valora las opiniones de los demás.", "Kinder" },
                { "Identifica al menos 3 o 4 elementos de la cultura dominicana.", "Kinder" },
                { "Comprensión de la importancia del manejo adecuado de los desechos.", "Kinder" },
                { "Usa diversas formas de los lenguajes de las artes y materiales de desecho con intención de comunicar sus ideas (reales o imaginarias), opiniones, emociones, sentimientos y experiencias.", "Kinder" },
                { "Realiza movimientos en diferentes direcciones, posiciones y velocidades, según indicaciones.", "Kinder" },
                { "Ubica objetos y realiza movimientos según indicaciones derecha- izquierda, con relación a su cuerpo.", "Kinder" },
                { "Explora y manipula objetos y los incorpora al juego o improvisaciones lúdicas al representar situaciones reales o imaginarias.", "Kinder" },
                { "Describe situaciones de su comunidad local a partir de observación, exploración y preguntas, con el apoyo de adultos.", "Kinder" },
                { "Participa en proyectos sobre problemas sencillos que afectan su comunidad.", "Kinder" },
                { "Colabora en la búsqueda de información con ayuda del adulto, sobre temas de interés de su entorno inmediato natural y fenómenos naturales.", "Kinder" },
                { "Ordena hechos sencillos de la realidad ocurridos en su entorno inmediato utilizando su imaginación.", "Kinder" },
                { "Predice acontecimientos explicando lo ocurrido de acuerdo con su conocimiento y experiencias previas.", "Kinder" },
                { "Asume progresivamente las reglas acordadas en algunos juegos..", "Kinder" },
                { "Sigue una secuencia en el uso de textos orales que escucha, en situaciones cotidianas de comunicación.", "Kinder" },
                { "Formula y responde a preguntas, al obtener informaciones sobre temas de su interés.", "Kinder" },
                { "Asume progresivamente normas de comunicación establecidas.", "Kinder" },
                { "Expresa ideas y sentimientos a través de imágenes sencillas creadas con masillas.", "Kinder" },
                { "Usa frases sencillas organizadas en forma lógica, crítica y creativa en la descripción de situaciones cotidianas y objetos de su entorno inmediato, y a través de ilustraciones.", "Kinder" },
                { "Describe situaciones problemáticas del entorno, ubicándolas de forma adecuada y creativa.", "Kinder" },
                { "Explora y manipula títeres y objetos y los incorpora al juego o improvisaciones lúdicas.", "Kinder" },
                { "Explora las posibilidades sonoras de su cuerpo, voz y de sonidos producidos al percutir, sacudir o frotar instrumentos musicales y objetos o materiales de su entorno.", "Kinder" },
                { "Entona canciones infantiles y las acompaña con instrumentos de percusión menor", "Kinder" },
                { "Imita situaciones reales e imaginaria a partir canciones, juegos de rondas o cuentos cortos.", "Kinder" },
                { "Participa en juegos grupales reglados de reglas, aceptando procedimientos acordados en el grupo.", "Kinder" },
                { "Realiza movimientos con algunas partes de su cuerpo de manera coordinada en el espacio parcial y total.", "Kinder" },
                { "Cuestiona, observa y explora su entorno natural cercano con ayuda de otras personas.", "Kinder" },
                { "Usa utensilios, artefactos de su entorno o recursos tecnológicos, al realizar experimentos sencillos.", "Kinder" },
                { "Registra los resultados de la exploración del entorno natural de forma oral, escrita o gráfica, de manera convencional o no.", "Kinder" },
                { "Integra de manera espontánea objetos del entorno cercano a sus juegos simulando situaciones reales o imaginarias..", "Kinder" },
                { "Responde a preguntas sobre situaciones o actividades que se desarrollan en su contexto inmediato..", "Kinder" },
                { "Expresa sus ideas sobre lo que observa y las actividades cotidianas que realiza", "Kinder" },
                { "Responde a preguntas sencillas sobre la idea general de un texto.", "Kinder" },
                { "Interpreta mensajes a partir de imágenes y símbolos, en textos sencillos y establece comparaciones progresivas en palabras que inician o terminan similar, a partir del sonido o la grafía.", "Kinder" },
                { "Lee progresivamente, de manera no convencional o convencional, imágenes y palabras en textos sencillos, comprendiendo su significado literal y utiliza algunas formas convencionales de lectura.", "Kinder" },
                { "Describe situaciones problemáticas del entorno, ubicándolas de forma adecuada y creativa", "Kinder" },
                { "Identifica y comparte sus gustos y preferencias con respeto, cortesía expresiones muy breves y sencillas.", "Kinder" },
                { "Valora a sus compañeros respetando sus ideas y sentimientos.", "Kinder" },
                { "Escucha con atención cuentos musicales, identificando los sonidos de voces e instrumentos.", "Kinder" },
                { "Mueve su cuerpo de forma espontánea, con estímulos sonoros de origen diverso.", "Kinder" },
                { "Reproduce patrones rítmicos sencillos con su cuerpo, su voz o instrumentos.", "Kinder" },
                { "Realiza desplazamientos en el espacio mostrando estabilidad y equilibrio en sus movimientos.", "Kinder" },
                { "Experimenta con su cuerpo movimientos de estiramiento, relajación y descanso, al finalizar actividades físicas.", "Kinder" },
                { "Participa en actividades de higiene y cuidado personal, con apoyo del adulto.", "Kinder" },
                { "Identifica algunas semejanzas y diferencias entre seres vivos de su entorno.", "Kinder" },
                { "Utiliza informaciones sobre personas, animales u objetos conocidos para apoyar sus explicaciones.", "Kinder" },
                { "Colabora y explica algunas medidas en actividades de protección y cuidado del entorno escolar, recursos, sus plantas y animales.", "Kinder" },
                { "Expresa a los demás sus acuerdos o desacuerdos ante situaciones o hechos cotidiano.", "Kinder" },
                { "Participa en la búsqueda y selección de alternativas al solucionar situaciones cotidianas", "Kinder" },
                { "Colabora en la puesta en búsqueda de la solución acordada de problemas sencillos o situaciones cotidianas que afectan a sí mismo o al grupo.", "Kinder" },
                { "Expresa con sus palabras ideas o información escuchadas o leídas en textos funcionales y literarios de manera no convencional o progresivamente convencional.", "Kinder" },
                { "Escribe su nombre de manera no convencional o progresivamente convencional.", "Kinder" },
                { "Reproduce o produce textos basados en situaciones reales e imaginarias de manera no convencional y progresivamente convencional, utilizando algunos recursos o medios tecnológicos o convencionales.", "Kinder" },

                #endregion

                #region Preprimaria

                { "Comunica sus ideas, pensamientos, emociones y experiencias con la intención de que otros comprendan el mensaje", "Preprimaria" },
                { "Utiliza técnicas y materiales en sus producciones para expresar sus emociones, sentimientos, ideas y experiencias", "Preprimaria" },
                { "Expresa ideas y sentimientos a través de producciones digitales en creación de imágenes, formas y sonidos", "Preprimaria" },
                { "Selecciona y combina colores primarios y secundarios, utilizándolos con intencionalidad en sus producciones plásticas bidimensionales y tridimensionales al representar ideas reales o imaginarias.", "Preprimaria" },
                { "Reproduce algunos elementos de la cultura dominicana y de otras culturas, utilizando elementos de los lenguajes artísticos.", "Preprimaria" },
                { "Reproduce o continua a partir de modelos, patrones rítmicos y melódicos con su cuerpo e instrumentos musicales.", "Preprimaria" },
                { "Comunica a través del movimiento una idea, sentimientos o experiencia incorporando objetos al juego o improvisaciones lúdicas.", "Preprimaria" },
                { "Identifica su lateralidad en la realización de acciones motrices y en la ejecución de diferentes tipos de formaciones (filas, hileras, columnas, círculos, entre otras).", "Preprimaria" },
                { "Expresa su disponibilidad corporal para la realización y disfrute de la actividad física", "Preprimaria" },
                { "Participa en pequeños experimentos utilizando elementos manipulables y seguros, realizando inferencias y registrando los resultados de manera convencional o no.", "Preprimaria" },
                { "Comunica resultados de la exploración del entorno natural de forma oral, escrita o gráfica", "Preprimaria" },
                { "Identifica algunas semejanzas y diferencias entre seres vivos de su entorno", "Preprimaria" },
                { "Interpreta imágenes, graficas o símbolos matemáticos no convencionales y convencionales que representan informaciones de su entorno inmediato", "Preprimaria" },
                { "Descubre orden lógico y secuencia numérica para completar una serie numérica hasta el 9 y realiza operaciones de adición y sustracción con objetos concretos y luego de forma semiconcreta utilizando representaciones numéricas no convencionales y convencionales para resolver problemas sencillos de la cotidianidad.", "Preprimaria" },
                { "Agrupa objetos por iguales características dadas por otras personas o criterios propios y ofrece explicaciones, así como relaciona una idea con otra al analizar situaciones cotidianas, aportando su opinión o conclusión.", "Preprimaria" },
                { "Sigue una secuencia en el uso de textos orales que escucha, en situaciones cotidianas de comunicació", "Preprimaria" },
                { "Formula y responde a preguntas, al obtener informaciones sobre temas de su interés", "Preprimaria" },
                { "Asume progresivamente normas de comunicación establecidas", "Preprimaria" },
                { "Conversa con otros y otras sobre distintos temas y situaciones, escuchando sus ideas y opiniones con interés.", "Preprimaria" },
                { "Comunica su opinión en situaciones, relatos o textos y ofrece al menos una o dos razones que la sustente.", "Preprimaria" },
                { "Colabora en la elaboración de un plan de solución sencillo sobre un problema identificado.", "Preprimaria" },
                { "Danza en forma espontánea, rítmica y con materiales diversos.", "Preprimaria" },
                { "Dibuja libremente con mayor precisión e intencionalidad comunicativa, agregando progresivamente elementos del lenguaje plástico a sus producciones.", "Preprimaria" },
                { "Crea figuras bidimensionales y tridimensionales utilizando el papel.", "Preprimaria" },
                { "Participa en juegos grupales reglados, aceptando y cumpliendo los procedimientos acordados en el grupo.", "Preprimaria" },
                { "Realiza movimientos con algunas partes de su cuerpo de manera coordinada en el espacio parcial y total", "Preprimaria" },
                { "Realiza movimientos en diferentes direcciones, posiciones y velocidades, según indicaciones", "Preprimaria" },
                { "Cuestiona, observa y explora su entorno natural al profundizar sobre temas de interés", "Preprimaria" },
                { "Usa las TIC y utensilios, artefactos de su entorno, al realizar experimentos y tareas cotidianas", "Preprimaria" },
                { "Aplica en pequeños grupos, pasos del método científico al realizar experimentos sencillos, con apoyo del adulto", "Preprimaria" },
                { "Realiza agrupaciones de objetos de acuerdo con uno o más atributos en la organización de informaciones y solución de situaciones cotidianas", "Preprimaria" },
                { "Reproduce o crea patrones matemáticos de tamaño, longitud, cantidad o posición, relacionados a una información, usando material concreto o formato digital", "Preprimaria" },
                { "Utiliza informaciones sobre personas, animales u objetos conocidos para apoyar sus explicaciones o creencias, así como identifica progresivamente actividades de la vida diaria que se encuentran organizadas por patrones.", "Preprimaria" },
                { "Responde a preguntas sencillas sobre la idea general de un texto", "Preprimaria" },
                { "Interpreta mensajes a partir de imágenes y símbolos, en textos sencillos y establece comparaciones progresivas en palabras que inician o terminan similar, a partir del sonido o la grafía", "Preprimaria" },
                { "Lee progresivamente, de manera no convencional o convencional, imágenes y palabras en textos sencillos, comprendiendo su significado literal y utiliza algunas formas convencionales de lectura", "Preprimaria" },
                { "Identifica a los miembros de su familia algunas costumbres, tradiciones y ocupaciones de algunos miembros de su comunidad", "Preprimaria" },
                { "Cumple con sus deberes realizando las actividades escolares que le son solicitadas", "Preprimaria" },
                { "Comenta sobre las historias, y personajes de su familia y la comunidad", "Preprimaria" },
                { "Describe la trama y la participación de algunos personajes de dramatizaciones y obras de títeres, identificando ideas, sentimientos o emociones.", "Preprimaria" },
                { "Maneja títeres sencillos, junto a compañeros y compañeras, contando historias para otras personas", "Preprimaria" },
                { "Representa situaciones reales e imaginarias a partir de poesías, canciones o cuentos", "Preprimaria" },
                { "Se desplaza en diferentes direcciones y posiciones con y sin instrumentos, manteniendo el equilibrio y el control postural, siguiendo secuencias, compases y ritmos compases.", "Preprimaria" },
                { "Experimenta con su cuerpo movimiento, estiramiento, respiración, relajación y descanso", "Preprimaria" },
                { "Cuida su cuerpo, el de los y las demás, practicando hábitos de higiene antes, durante y después de la realización de actividades físicas.", "Preprimaria" },
                { "Describe algunos eventos y fenómenos naturales, así como las medidas de seguridad", "Preprimaria" },
                { "Colabora en actividades de manejo adecuado de desechos y reciclaje", "Preprimaria" },
                { "Participa en proyectos sobre problemáticas sencilla que afectan su comunidad y propone acciones para el cuidado del medio ambiente, las plantas y los animales.", "Preprimaria" },
                { "Organiza sus ideas para realizar juegos y actividades de la vida cotidiana, así como participa en juegos que abordan procesos lógicos  respetando las reglas establecidas y a los participantes.", "Preprimaria" },
                { "Participa en la búsqueda y selección de alternativas al solucionar problemas sencillos, y colabora en la elaboración de un plan de solución sencillo sobre un problema identificado.", "Preprimaria" },
                { "Expresa con sus palabras ideas o información escuchadas o leídas en textos funcionales y literarios de manera no convencional o progresivamente convencional", "Preprimaria" },
                { "Escribe su nombre de manera no convencional o progresivamente convencional", "Preprimaria" },
                { "Reproduce o produce textos basados en situaciones reales e imaginarias de manera no convencional y progresivamente convencional, utilizan algunos recursos o medios digitales", "Preprimaria" },

                #endregion               

                #region Sexto 

                #endregion

            };

            int idCounter = 1;

            foreach (var grade in gradeData)
            {
                var viewModel = new SaveAchievementIndicatorsViewModel
                {
                    Id = idCounter,
                    Description = grade.Key,
                    AssociatedGrades = grade.Value,
                    CreatedBy = "Admin",
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LastModifiedBy = "Admin"
                };
                await indicatorsService.Add(viewModel);
                idCounter++;
            }

            var competencias = new Dictionary<string, AchievementIndicatorInfo>
            {
                 #region Segundo

                    { "<strong>Comunicativa</strong><br> Se expresa adecuadamente en los ámbitos familiar, escolar y social, mediante un género textual (funcional o literario), a fin de demostrar conocimiento en el uso de su lengua oral o escrita, a través de medios y recursos convenientes.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "LENESP" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica </strong><br>Produce textos orales y escritos en demostración de razonamiento lógico sobre indagaciones en fuentes bibliográficas y/o investigaciones científicas sencillas que realiza; para aportar soluciones a problemas familiares, estudiantiles, sociales, y su divulgación a través de medios tecnológicos y de otros tipos.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "LENESP" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Utiliza textos variados para conocer y cuestionar las prácticas sociales de ciudadanía; con la finalidad de promover valores universales y humanísticos, así como la divulgación y promoción de situaciones de salud y medio ambiente, mediante el uso de herramientas tecnológicas, entre otras.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "LENESP" } },

                    { "<strong>Comunicativa</strong><br> Explica, en forma oral y escrita, sus ideas sobre conceptos y situaciones con contenido matemático, relacionando su lenguaje diario con el lenguaje y los símbolos matemáticos.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "MAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica </strong><br> Utiliza el enfoque de resolución de problemas para entender los contenidos matemáticos, estableciendo e investigando conjeturas para justificar argumentos y pruebas relacionadas con éstos, de acuerdo con su desarrollo cognitivo, apoyándose en recursos concretos, y en recursos y soportes digitales, para complementar el trabajo matemático.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "MAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Respeta los puntos de vista y argumentos de los demás sobre situaciones que implican conocimientos matemáticos referidos al contexto social y ambiental.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "MAT" } },
                    
                    { "<strong>Comunicativa</strong><br> Compara conceptos e informaciones de su historia familiar y comunitaria, con la finalidad de reflexionar en forma crítica sobre sus ideas y soluciones creativas a situaciones diversas, utilizando la tecnología.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "CIESOC" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica </strong><br> Comprende en forma crítica conceptos y situaciones problemáticas del espacio natural y social de su entorno comunitario, con la finalidad de aportar ideas a las soluciones creativas utilizando tecnología.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "CIESOC" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud  </strong><br> Emplea actitudes de respeto a sí mismo y a las demás personas en cualquier espacio; con la finalidad de construir una ciudadanía basada en la participación democrática, la exigencia de sus derechos, el cumplimiento de sus deberes y el cuidado de su entorno natural y social.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "CIESOC" } },
                    
                    { "<strong>Comunicativa</strong><br>Ofrece explicaciones de observaciones, exploraciones, y cuestionamientos de fenómenos naturales a partir de su contexto próximo y experimentado en ciencias de la vida, físicas, de la tierra y el universo.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica </strong><br> Aplica procedimientos organizados y creativos explorando, manipulando, construyendo y haciéndose consciente de sus cuestionamientos a partir de observación y medición llevando a cabo de vivencias, experimentos, exploraciones y observaciones guiadas.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud </strong><br> Asume una actitud preventiva y en armonía en sí mismo, con los demás, con su entorno y como parte de los seres vivos, tomando acciones básicas y proactivas en atención a su bienestar y uso sostenibles de los recursos naturales. ", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "CIEDELANAT" } },
                    
                    { "<strong>Comunicativa</strong><br> Utiliza las acciones motrices complejas en situaciones de juego, con el objeto de expresar y comunicar sus sentimientos, emociones y estados de ánimo.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "EDUFÍS" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica </strong><br> Resuelve en forma creativa situaciones de juego simples y complejas; en procura de evidenciar un desempeño motriz y dominio postural eficaz a partir de sus condiciones físicas naturales.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "EDUFÍS" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud </strong><br> Aplica su disponibilidad corporal en juegos colectivos, con el propósito de disfrutar la actividad motriz alcanzando el bienestar propio y de los demás, protegiendo su salud y el medio ambiente de su entorno.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "EDUFÍS" } },
                    
                    { "<strong>Comunicativa </strong><br>Reconoce valores en los principales hechos de la historia de Jesús de Nazaret, con el objetivo de identificarlos en su familia, la escuela y la cultura dominicana, a través de testimonio de personas y textos bíblicos, con respeto y alegría.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Contrasta normas del juego, el trato a los demás y el uso de las tecnologías para su bienestar y el fortalecimiento de las relaciones en su entorno comunitario y escolar, con autonomía y naturalidad partiendo de las enseñanzas de Jesús de Nazaret.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject =  "FORINTHUMYREL" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud </strong><br> Distingue acciones que favorecen el cuidado y protección de su cuerpo, el de los demás y la naturaleza como dones de Dios con la finalidad de mantenerlos saludables, con autonomía, creatividad y valoración.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject =  "FORINTHUMYREL" } },
                    
                    { "<strong>Comunicativa </strong><br> Usa imágenes, movimientos, textos, gestos, colores y materiales diversos, a fin de comunicar ideas, emociones, sentimientos y vivencias, en contextos diversos.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "EDUART" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica  </strong><br> Incorpora en sus expresiones artísticas, características de personajes, instrumentos, objetos y manifestaciones de su comunidad; con la finalidad de usarlos como elementos de comunicación de ideas, sentimientos y vivencias en la solución de problemas de manera creativa en distintos contextos.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "EDUART" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud   </strong><br> Integra ideas y situaciones de su entorno natural y cultural en sus creaciones; con el objeto de reafirmar su identidad personal y social, así como el interés por su salud y el medio ambiente.", new AchievementIndicatorInfo { Grade = "Segundo", CodeSubject = "EDUART"  } },
                    { "Expresa sus sentimientos, emociones e ideas de forma gráfica o escrita, al escuchar narraciones, mensajes, cuentos, canciones, poesías leídas por otras personas, de manera no convencional.", new AchievementIndicatorInfo { Grade = "sin datos", CodeSubject = null } },
               
                #endregion

                 #region Tercero

                    #region Lengua Española

                    { "<strong>Comunicativa</strong><br> Comunica sus ideas con claridad en contextos diversos, empleando un modelo textual (funcional o literario), con el fin de evidenciar conocimiento y uso de la lengua oral o escrita, mediante herramientas y recursos variados.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject =  "LENESP" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Expone con creatividad y de manera crítica las conclusiones sobre la solución de problemas, obtenidas en investigaciones científicas, a través de un género textual conveniente y con uso de recursos variados, respetando la diversidad de opiniones.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject =  "LENESP" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Describe problemas sociales de manera colectiva (antidemocráticos, discriminación, entre otros), a través de textos orales y escritos; a fin de solucionarlos y canalizar emociones, sentimientos, relaciones humanas, así como la preservación de la salud, el ecosistema, mediante el uso de medios y recursos diversos.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject =  "LENESP" } },

                    #endregion

                    #region Matemática

                    { "<strong>Comunicativa</strong><br> Representa -Leyendo, escribiendo, interpretando y discutiendo- su comprensión de los enunciados de problemas matemáticos, los conceptos y propiedades de las operaciones, para establecer las relaciones entre ellos y con el contexto circundante.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "MAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Utiliza en forma sistemática estrategias de resolución de problemas, verificando e interpretando sus resultados con relación a la situación de problema presentada, y justificando con su lenguaje cotidiano, procesos de razonamientos propios, acerca de una demostración, un algoritmo o un resultado matemático, de forma inductiva y utilizando recursos concretos e integrando las tecnologías digitales.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "MAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Interpreta posibles soluciones a situaciones del entorno y el Medioambiente a partir de sus conocimientos matemáticos.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "MAT" } },

                    #endregion

                    #region Ciencias Sociales

                    { "<strong>Comunicativa</strong><br> Evalúa distintas fuentes geográficas e históricas, en el análisis de acontecimientos; con la finalidad de comprender el pasado y reconocer el espacio geográfico que ocupa su municipio, provincia, región y país.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "CIESOC" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Utiliza procedimientos científicos y tecnológicos en el análisis crítico de fenómenos geográficos, hechos históricos y culturales de la provincia, región y país; con la finalidad de realizar propuestas lógicas y creativas.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "CIESOC" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Ejemplifica en sus actuaciones el respeto a sí mismo y a las demás personas; con la finalidad de promover relaciones democráticas y armoniosas, el cuidado a su entorno natural y la construcción de una cultura de paz en el municipio y región donde vive.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "CIESOC" } },

                    #endregion

                    #region Ciencias de la Naturaleza

                    { "<strong>Comunicativa</strong><br> Ofrece explicaciones de observaciones, exploraciones, y cuestionamientos de fenómenos naturales a partir de su contexto próximo y experimentado en ciencias de la vida, físicas, de la tierra y el universo.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Aplica procedimientos organizados y creativos explorando, manipulando, construyendo y haciéndose consciente de sus cuestionamientos a partir de observación y medición llevando a cabo de vivencias, experimentos, exploraciones y observaciones guiadas.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Asume una actitud preventiva y en armonía en sí mismo, con los demás, con su entorno y como parte de los seres vivos, tomando acciones básicas y proactivas en atención a su bienestar y uso sostenibles de los recursos naturales.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "CIEDELANAT" } },

                    #endregion

                    #region Educación Física

                    { "<strong>Comunicativa</strong><br> Establece comunicación armónica con las demás personas y su entorno social y cultural, mediante la realización de acciones motrices más complejas; con la finalidad de expresar los sentimientos, emociones y estados de ánimo que experimenta.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject =  "EDUFÍS"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Maneja la noción de su esquema corporal y de relaciones espaciales y temporales en entornos variados; con la finalidad de resolver con eficacia en situaciones simples y complejas mediante acciones motrices individuales, mostrando dominio postural.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject =  "EDUFÍS"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Realiza actividades motrices en interacción con el grupo, a partir de su disponibilidad corporal; en procura del bienestar propio y colectivo, mostrando respeto y evitando situaciones de riesgo para su salud, de los demás y el medio ambiente de su entorno.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject =  "EDUFÍS"  } },

                    #endregion

                    #region Formación Integral Humana y Religiosa

                    { "<strong>Comunicativa</strong><br> Plantea sus ideas sobre las diferentes etapas de la vida de Jesús Nazaret, con la finalidad de aplicar las enseñanzas que de ellas se desprenden en su familia con creatividad, admiración, tomando en cuenta textos de los evangelios.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "FORINTHUMYREL"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Practica normas de convivencia en el juego, el trato a los demás y el uso de las tecnologías, con el fin de favorecer el bienestar de sí mismo y cultivar buenas relaciones con sus compañeros, familiares y demás personas, con espontaneidad, empeño y libertad desde la propuesta de Jesús de Nazaret.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "FORINTHUMYREL"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Demuestra valoración de la vida humana y de la creación reconociendo que vienen de Dios, con la finalidad de promover en sus acciones el cuidado de la vida y la protección de su entorno natural con creatividad, respeto, admiración y disfrute.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "FORINTHUMYREL"  } },

                    #endregion

                    #region Educación Artística

                    { "<strong>Comunicativa</strong><br> Utiliza elementos de los lenguajes artísticos, con el fin de comunicar ideas, conceptos y sentimientos.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "EDUART" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Utiliza elementos de los lenguajes artísticos, incorporando aspectos de la ciencia y la tecnología, con la finalidad de aplicar soluciones creativas en sus trabajos.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "EDUART" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Participa en actividades artísticas grupales, mostrando respeto por la diversidad de opiniones y formas de expresiones al emplear el reciclaje, sonidos y movimientos a fin de promover el cuidado de la salud y del medio ambiente.", new AchievementIndicatorInfo { Grade = "Tercero", CodeSubject = "EDUART" } },

                    #endregion                

                 #endregion

                 #region Cuarto 
            
                    #region Lengua Española

                    { "<strong>Comunicativa</strong><br> Se comunica en diferentes contextos mediante un género textual adecuado, con el propósito de expresar sus ideas y pensamientos, haciendo uso de medios y recursos apropiados, de forma individual o colectiva.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "LENESP" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Emplea textos variados, orales y escritos, en la construcción de nuevos conocimientos sobre temas y problemas de su vida social, con la finalidad de solucionarlos, a través de investigaciones científicas y el uso de medios y recursos.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "LENESP" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Usa textos orales y escritos en demostración de conocimiento sobre las relaciones socioculturales, a fin de fortalecer su conocimiento y percepción del mundo, mediante temas relacionados con salud, ambiente y comunidad, con el uso de medios y recursos tecnológicos y de otros tipos.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "LENESP" } },

                    #endregion

                    #region Matemática

                    { "<strong>Comunicativa</strong><br> Interpreta ideas expresadas en lenguaje matemático con la finalidad de discutir situaciones de problemas del contexto.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "MAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Formula con sentido lógico ideas matemáticas válidas, para proponer solución a situaciones del mundo fuera del aula expresadas de forma verbal, numérica, gráfica, geométrica o simbólica.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "MAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Reconoce las normas de convivencia y del trabajo en equipo, respetando las ideas de compañeros para llegar a acuerdos sobre los temas matemáticos desarrollados.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "MAT" } },

                    #endregion

                    #region Ciencias Sociales

                    { "<strong>Comunicativa</strong><br> Utiliza fuentes de información seguras sobre elementos físicos y políticos de la Geografía del Caribe y las Antillas, con la finalidad de expresar los vínculos que unen la región de América.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "CIESOC"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Identifica problemas en hechos históricos de los siglos XVI, XVII, XVIII y XIX, con el propósito de relacionarlos en forma lógica, creativa y crítica con el espacio geográfico en el que ocurrieron, expresando su opinión.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "CIESOC"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Actúa democráticamente en defensa del patrimonio histórico, natural y cultural de la República Dominicana, con la finalidad de construir una ciudadanía basada en el respeto propio y a los demás, la convivencia pacífica, teniendo en cuenta las diferencias individuales, sociales y culturales, y preservación de la salud y de los recursos naturales del país.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "CIESOC"  } },

                    #endregion

                    #region Ciencias de la Naturaleza

                    { "<strong>Comunicativa</strong><br> Ofrece explicaciones científicas y tecnológicas a partir de análisis, observaciones, medición, modelos y experimentación de fenómenos naturales fundamentales en contexto próximo o experimentado o modelado en ciencias de la vida, físicas, de la tierra y el universo.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Aplica organizados y lógicos procedimientos científicos y tecnológicos, que analiza mientras explora o experimenta, simula o construye, haciéndose consciente de sus cuestionamientos e inferencias a partir de su observación y medición, llevando a cabo experimentos, proyectos, exploraciones y observaciones guiadas.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Asume una actitud preventiva, creativa, curiosa, colaborativa, responsable y en armonía integral en sí mismo, con los demás, con su entorno y como parte de los seres vivos, tomando acciones básicas y proactivas en atención a su bienestar y uso sostenible de los recursos.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "CIEDELANAT" } },

                    #endregion

                    #region Lenguas Extranjeras (Inglés)

                    { "<strong>Comunicativa</strong><br> Comprende y expresa información e ideas en forma oral y escrita en inglés, utilizando un repertorio muy limitado de vocabulario y expresiones muy breves y sencillas tanto para identificarse y como para describir su entorno inmediato en situaciones concretas de comunicación. ", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "ING" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Interactúa sobre informaciones básicas, problemas y situaciones cotidianas de su entorno inmediato, relativas a temas científicos y tecnológicos, tales como la descripción de seres vivos y la ubicación en el espacio, utilizando un repertorio ensayado de expresiones muy breves y sencillas de manera lógica y creativa en inglés, con ayuda de su interlocutor.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "ING" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud </strong><br> Se relaciona con las demás personas y su entorno, interactuando en inglés con expresiones muy breves y sencillas, en un plano de cortesía y respeto, cuidado de la salud y el medio ambiente, y valoración de las diferencias individuales y la identidad social y cultural propias y de las demás personas.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "ING" } },

                    #endregion

                    #region Educación Física

                    { "<strong>Comunicativa</strong><br> Explora sus condiciones y características corporales, con el objeto de usarlas como medio de expresión y comunicación de sentimientos, emociones y estados de ánimo, a través de sus acciones motrices.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject =  "EDUFÍS"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Identifica sus gestos y sus posibilidades de movimientos corporales con la finalidad de expresar y comunicar sus sentimientos, emociones y estados de ánimo de forma intencional en la realización de juegos motores populares y tradicionales.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject =  "EDUFÍS"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Domina sus gestos y movimientos corporales, con el fin de expresar y comunicar de forma intencional sus sentimientos, emociones y estados de ánimo, en relación con el entorno natural y social, en la creación de y ejecución de variantes de juegos populares y tradicionales.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject =  "EDUFÍS"  } },

                    #endregion

                    #region Formación Integral Humana y Religiosa

                    { "<strong>Comunicativa</strong><br> Expresa que su cuerpo cambia, se comunica y que necesita cuidado y protección, con la finalidad de reconocer su valor y el de los demás en diferentes contextos, tomando en cuenta que es criatura de Dios, hecho a su imagen y semejanza.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "FORINTHUMYREL"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Distingue los aportes del trabajo humano y de las ciencias y tecnologías en la familia, la escuela y la comunidad, con el fin de reconocer en estas contribuciones la expresión del quehacer de Dios, con respeto, criticidad y asertividad.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "FORINTHUMYREL"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Identifica las características de su desarrollo físico, cognitivo, afectivo, sexual y espiritual para afianzar el amor a sí mismo, a los demás y a la naturaleza tomando como referencia el amor de Dios expresado en la persona de Jesús de Nazaret, con actitud de respeto, autonomía, libertad, asertividad y agradecimiento.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "FORINTHUMYREL"  } },

                    #endregion

                    #region Educación Artística

                    { "<strong>Comunicativa</strong><br> Utiliza símbolos creados a partir del sonido, el gesto, el movimiento y la imagen, con el fin de integrarlos como elementos comunicativos de los lenguajes artísticos.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "EDUART" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Analiza críticamente distintas manifestaciones artísticas en medios físicos y virtuales al crear o disfrutarlas o al describir elementos constitutivos presentes en ellas, a fin de resolver problemáticas de su contexto.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "EDUART" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Plasma creativamente formas y temas inspirados en su contexto, haciendo uso adecuado de la voz, el cuerpo, recursos y técnicas, al comunicar sus ideas y sentimientos de dominicanidad.", new AchievementIndicatorInfo { Grade = "Cuarto", CodeSubject = "EDUART" } },

                    #endregion

                #endregion

                 #region Quinto 

                    #region Lengua Española

                    { "<strong>Comunicativa</strong><br> Se comunica de manera apropiada en diferentes ámbitos sociales según su capacidad, a través de textos funcionales y literarios, con la finalidad de manifestar comprensión en el uso y dominio de la lengua, utilizando medios y recursos diversos.", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "LENESP" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Produce textos específicos orales y escritos en demostración de razonamiento, a partir de las investigaciones científicas que realiza, con la finalidad de aportar soluciones a problemas sociales, y su publicación a través de medios tecnológicos y de otros tipos.", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "LENESP" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Expone de forma oral o escrita sus conocimientos sobre prácticas sociales, a fin de promover valores universales y espirituales, así como la preservación de la salud y el ambiente, utilizando herramientas tecnológicas, entre otras", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "LENESP" } },

                    #endregion

                    #region Matemática

                    { "<strong>Comunicativa</strong><br> Explica sus ideas a partir de sus conocimientos matemáticos para resolver situaciones de la vida cotidiana", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "MAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Utiliza estrategias para resolución de problemas matemáticos, mostrando razonamiento en contextos espaciales, mediante proporciones, a partir de gráficas, y apoyándose en recursos concretos y en recursos y soportes digitales para complementar el trabajo matemático", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "MAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Respeta la diversidad de opiniones y argumentos de los demás sobre situaciones que implican conocimientos matemáticos referidos al contexto social y ambiental", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "MAT" } },

                    #endregion

                    #region Ciencias Sociales

                    { "<strong>Comunicativa</strong><br> Selecciona fuentes seguras y confiables, con el propósito de comprender y comunicar informaciones sobre geografía e historia de la isla de Santo Domingo y el continente americano, con la finalidad de respetar la autoría de las informaciones, comprenderlas y comunicarlas", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "CIESOC"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Analiza conceptos y situaciones referidos a la geografía, la historia como ciencia, el continente americano, el Caribe y los procesos históricos de República Dominicana, con la finalidad de plantear su punto de vista sobre sus hallazgos de manera lógica, crítica y creativa", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "CIESOC"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Defiende actitudes democráticas, de respeto a los símbolos patrios del país, la valoración del patrimonio histórico, natural y cultural dominicano y de Latinoamérica, con la finalidad de construir una ciudadanía respetuosa de los derechos humanos y del cuidado de su salud, el Medioambiente y defensora de la cultura de paz", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "CIESOC"  } },

                    #endregion

                    #region Ciencias de la Naturaleza

                    { "<strong>Comunicativa</strong><br> Ofrece explicaciones científicas y tecnológicas a partir de analizar y evaluar preguntas o hipótesis de observaciones, medición, modelos y experimentación de fenómenos naturales en contexto próximo o experimentado o modelado en ciencias de la vida, físicas, de la tierra y el universo", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Aplica organizados y sistemáticos procedimientos científicos y tecnológicos, que evalúa mientras explora o experimenta, simula o construye, haciéndose consciente de sus cuestionamientos e inferencia a partir de su observación y medición, llevando a cabo vivencias, experimentos, proyectos, exploraciones y observaciones guiadas", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Asume una actitud preventiva, creativa, curiosa, colaborativa, responsable y en armonía integral en sí mismo, con los demás, con su entorno y como parte de los seres vivos, tomando acciones básicas y proactivas en atención a su bienestar y uso sostenible", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "CIEDELANAT" } },

                    #endregion

                    #region Lenguas Extranjeras (Inglés)

                    { "<strong>Comunicativa</strong><br> Comprende y expresa información e ideas en forma oral o escrita en inglés, utilizando un repertorio limitado de frases y oraciones muy breves y sencillas para compartir información básica propia, de otras personas y de su entorno inmediato", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "ING" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Interactúa sobre algunas informaciones, problemas y situaciones cotidianas de su entorno inmediato, relativas a temas científicos y psicológicos, tales como la descripción de aspectos comunes del cuerpo humano, forma de ser de las personas y las condiciones meteorológicas, utilizando un repertorio limitado de expresiones muy breves y sencillas de manera lógica y creativa en inglés, con ayuda de su interlocutor", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "ING" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud </strong><br> Se relaciona con las demás personas y su entorno respetuosamente, reconociendo las diferencias individuales y culturales propias y de otros, interactuando en inglés de forma muy breve y sencilla en situaciones cotidianas concretas donde muestra preferencias por opciones que impactan positivamente la salud y el medio ambiente", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "ING" } },

                    #endregion

                    #region Educación Física

                    { "<strong>Comunicativa</strong><br> Identifica sus gestos y sus posibilidades de movimientos corporales con la finalidad de expresar y comunicar sus sentimientos, emociones y estados de ánimo de forma intencional en la realización de juegos motores populares y tradicionales", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject =  "EDUFÍS"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Utiliza sus capacidades físicas naturales en situaciones variadas (juegos, deportes, vida cotidiana), con el propósito de desarrollar diferentes niveles de desempeño motriz para el logro de la eficacia motora progresiva", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject =  "EDUFÍS"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Emplea sus habilidades motrices y capacidades físicas en el logro de metas compartidas en su entorno escolar y comunitario, con la finalidad de contribuir a la creación de relaciones pacíficas en la realización de tareas apropiadas a su etapa de desarrollo, mostrando respeto por los demás, el medio ambiente y por las reglas establecidas", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject =  "EDUFÍS"  } },

                    #endregion

                    #region Formación Integral Humana y Religiosa

                    { "<strong>Comunicativa</strong><br> Comunica la importancia de los cambios que se dan en su cuerpo, sus deberes y derechos a fin de construir relaciones de respeto y equidad en su entorno familiar, escolar y social, tomando en cuenta los derechos consignados sobre la niñez y su valor como hijo e hija de Dios", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "FORINTHUMYREL"  } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Analiza la contribución y perjuicios del trabajo humano, las ciencias y las tecnologías en el ámbito social, cultural y espiritual de las personas a fin de mejorar la creación de Dios, con creatividad, sentido ético y responsabilidad", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "FORINTHUMYREL"  } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Propone acciones que favorecen el cuidado a los demás y al entorno natural como casa común en la familia y la escuela a partir de la vida y las enseñanzas de Jesús de Nazaret para afianzar sus principios morales, su autoestima con respeto, responsabilidad, asertividad y gratitud", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "FORINTHUMYREL"  } },

                    #endregion

                    #region Educación Artística

                    { "<strong>Comunicativa</strong><br> Utiliza las posibilidades expresivas de su cuerpo y su voz, así como elementos plásticos, gestuales, sonoros y visuales, con la finalidad de representar ideas, emociones, vivencias propias y de otras personas", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "EDUART" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Usa crítica y creativamente elementos de la tecnología, la ciencia y la cultura, a fin de integrarlos en sus investigaciones y expresiones artísticas", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "EDUART" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Identifica distintas prácticas presentes en su comunidad, orientadas al cuidado de salud, de la naturaleza y del patrimonio cultural, con la finalidad de integrarlas en sus expresiones artísticas", new AchievementIndicatorInfo { Grade = "Quinto", CodeSubject = "EDUART" } },

                    #endregion

                 #endregion                         

                 #region Sexto 

                    #region Lengua Española

                    { "<strong>Comunicativa</strong><br> Comunica sus ideas, pensamientos y sentimientos con fluidez, mediante un modelo textual conveniente, en variadas situaciones y contextos, con el fin de demostrar conocimiento y uso adecuado de su lengua, a través de diferentes medios y recursos.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "LENESP" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Elabora textos orales y escritos con creatividad y criticidad según las conclusiones de los problemas abordados en investigaciones, y las publica a través de medios variados.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "LENESP" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Caracteriza problemas sociales a través de textos orales y escritos, con la finalidad de solucionarlos, canalizando emociones, sentimientos, relaciones humanas, así como la preservación de la salud y el ambiente, mediante el uso de recursos diversos.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "LENESP" } },

                    #endregion

                    #region Matemática

                    { "<strong>Comunicativa</strong><br> Interpreta textos, leyendo, escribiendo y discutiendo en forma comprensiva sus ideas matemáticas para resolver problemas de su contexto.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "MAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Aplica sus conocimientos matemáticos a la resolución de problemas abiertos y tareas ampliadas de resolución de problemas apoyándose en las tecnologías digitales.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "MAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Modela posibles soluciones a situaciones del contexto social y el medio ambiente a partir de sus conocimientos matemáticos.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "MAT" } },

                    #endregion

                    #region Ciencias Sociales

                    { "<strong>Comunicativa</strong><br> Comprueba la autoría de informaciones sobre la geografía, su clasificación e importancia; así como los aspectos geográficos e históricos de las de las civilizaciones antiguas y modernas, con la finalidad de producir valoraciones e informaciones científicas.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "CIESOC" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Analiza acontecimientos históricos relacionados con las civilizaciones antiguas y modernas, la historia de América durante los siglos XVIII y XIX, situaciones políticas, sociales, económicas y culturales del mundo, especialmente y la República Dominicana, ocurridos durante la primera y segunda mitad del siglo XIX y el XX; con la finalidad de comprender las causas que dieron origen al problema y expresar su opinión en forma crítica y creativa, usando la tecnología.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "CIESOC" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Promueve en sus actuaciones sociales, relaciones democráticas, la conservación del patrimonio histórico, natural y cultural dominicano y del mundo; con la finalidad de construir una ciudadanía consciente de sus deberes y derechos, y de los derechos de las demás personas, promotora de la interculturalidad y defensora de la paz en su contexto cercano y en el mundo.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "CIESOC" } },

                    #endregion

                    #region Ciencias de la Naturaleza

                    { "<strong>Comunicativa</strong><br> Ofrece explicaciones científicas y tecnológicas a partir de analizar, evaluar y crear preguntas o hipótesis de observaciones, medición, modelos y experimentación de fenómenos naturales en contexto próximo o experimentado o modelado en ciencias de la vida, físicas, de la tierra y el universo.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Aplica organizados, sistemáticos y creativos procedimientos científicos y tecnológicos, que analiza y evalúa mientras explora o experimenta, simula o construye, haciéndose consciente de sus cuestionamientos e inferencia a partir de su observación y medición llevando a cabo de vivencias, experimentos, proyectos, exploraciones y observaciones guiadas.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "CIEDELANAT" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Asume una actitud preventiva, autónoma, autoconsciente, creativa, innovadora, crítica, de apertura, investigadora, colaborativa, solidaria, perseverante, responsable y en armonía integral en sí mismo, con los demás, con su entorno y como parte de los seres vivos, tomando acciones básicas y proactivas en atención a su bienestar y uso sostenible de los recursos.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "CIEDELANAT" } },

                    #endregion

                    #region Lenguas Extranjeras (Inglés)

                    { "<strong>Comunicativa</strong><br> Comprende y expresa ideas, sentimientos y necesidades tanto propias como de otras personas en el idioma inglés, utilizando un repertorio básico de vocabulario, frases y oraciones muy breves y sencillas en distintas situaciones cotidianas de comunicación.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "ING" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Interactúa sobre algunas informaciones, problemas y situaciones cotidianas de su entorno inmediato, relativas a temas científicos, psicológicos y económicos, tales como la adquisición de bienes y servicios y la descripción de sentimientos, necesidades y dolencias de las personas, utilizando un repertorio básico de expresiones muy breves y sencillas de manera lógica y creativa en inglés, con ayuda de su interlocutor.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "ING" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud </strong><br> Interactúa con asertividad y actitud de respeto por las diferencias individuales y culturales propias y las de las demás personas y su entorno, mostrando preferencias por las opciones que impactan positivamente la salud y el medio ambiente al comunicarse en inglés de forma muy breve y sencilla en situaciones cotidianas de comunicación.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "ING" } },

                    #endregion

                    #region Educación Física

                    { "<strong>Comunicativa</strong><br> Domina sus gestos y movimientos corporales, con el fin de expresar y comunicar de forma intencional sus sentimientos, emociones y estados de ánimo, en relación con el entorno natural y social, en la creación de y ejecución de variantes de juegos populares y tradicionales.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "EDUFÍS" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Aplica sus habilidades motrices y capacidades físicas en actividades motrices progresivas, a los fines de alcanzar la eficacia motora en situaciones creativas de juego, apoyadas en herramientas tecnológicas de la vida cotidiana.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "EDUFÍS" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Establece relaciones armoniosas en su grupo de juegos y de otras acciones motrices; con el objetivo de actuar con sentido de respeto a la diversidad individual, social y cultural, aportando a la inclusión y la convivencia responsable en su entorno.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "EDUFÍS" } },

                    #endregion

                    #region Formación Integral Humana y Religiosa

                    { "<strong>Comunicativa</strong><br> Promueve el valor de su cuerpo, el derecho de la niñez a ser protegida de todo tipo de maltrato y explotación a fin de promover su bienestar y el de los demás, con esmero, respeto y agradecimiento a Dios.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Promueve el valor del trabajo humano, el progreso de las ciencias y las tecnologías como expresión del amor de Dios y el desarrollo de los pueblos a fin de reconocer el esfuerzo, sacrificio de las personas que trabajan, con responsabilidad, respeto y asertividad.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Diseña y desarrolla acciones que favorecen su desarrollo físico, cognitivo, afectivo-sexual y espiritual, el cuidado y protección del entorno natural, a fin de buscar soluciones a situaciones que se viven en la familia, la escuela y la comunidad a partir de la vida y valores de Jesús de Nazaret, con asertividad, responsabilidad, respeto y gratitud.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "FORINTHUMYREL" } },

                    #endregion

                    #region Educación Artística

                    { "<strong>Comunicativa</strong><br> Aplica diversas técnicas artísticas con la finalidad de comunicar ideas, sentimiento y vivencias, ejercitando la imaginación creadora.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "EDUART" } },
                    { "<strong>Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Científica y Tecnológica</strong><br> Realiza muestras, físicas o digitales, de creaciones artísticas y culturales, como resultado de sus investigaciones y de los medios y técnicas explorados, con la finalidad de comprender situaciones o problemas de su entorno.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "EDUART" } },
                    { "<strong>Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud</strong><br> Recrea en sus obras personajes, temas, y tradiciones, valorando la trascendencia a través del tiempo de los artistas y sus obras, por medio de la inclusión de eventos relacionados con el arte y la cultura, en la programación de su tiempo de ocio.", new AchievementIndicatorInfo { Grade = "Sexto", CodeSubject = "EDUART" } },

                    #endregion

                 #endregion                         

                 #region Primero Secundaria 

                    #region Lengua Española

                    { "<strong> Comunicativa.</strong><br> Se comunica con claridad en diferentes contextos, siguiendo los procesos de compresión y producción oral y escrita, al emplear un tipo de texto.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Utiliza secuencias argumentativas en discursos orales y escritos, creando nuevos conocimientos a partir del proceso de comprensión de textos, relacionados con temas y problemas sociales de su realidad.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },
                    { "<strong> Ética y Ciudadana. </strong><br>  Analiza textos variados orales o escritos que ponen de relieve hechos y tradiciones históricas relevantes, identificando nuevas relaciones sociales al reconocer y valorar el patrimonio natural y sociocultural dominicano.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },
                    { "<strong>  Científica y Tecnológica.</strong><br> Demuestra conocimiento de procesos investigativos científicos sencillos y del uso de tecnología, a través de textos científicos y especialmente los de secuencia expositivo-explicativa.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },
                    { "<strong>  Resolución de Problemas.</strong><br> Identifica problemas de su vida estudiantil o cotidiana, a través de un tipo de texto específico y apropiado, como punto de partida para su estudio y solución.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },
                    { "<strong>  Ambiental y de la Salud.</strong><br> Explica situaciones sobre salud, medioambiente y de la comunidad, mediante textos de diferentes secuencias y géneros, a través de herramientas tecnológicas y otros medios.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },
                    { "<strong>  Desarrollo Personal y Espiritual.</strong><br> Demuestra conocimiento y comprensión de sí mismo y de los demás al expresar su percepción, a través de un tipo de texto, favorable a las situaciones y a las personas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "LENESP" } },

                    #endregion

                    #region Inglés

                    { "<strong> Comunicativa. </strong><br> Comprende y expresa ideas, sentimientos y valores culturales en distintas situaciones de comunicación de forma breve y sencilla, con la intención de informar, dar instrucciones, describir y opinar, con relación a necesidades de temas cotidianos. ", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Interactúa con el propósito de comparar y narrar hechos y situaciones mediante el razonamiento lógico-verbal en distintas situaciones de comunicación. ", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },
                    { "<strong> Ética y Ciudadana. </strong><br>  Se comunica en intercambios breves y sencillos, con el fin de identificar diferencias individuales y la identidad social y cultural propia y de otros países, participando en un plano de respeto y colaboración.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },
                    { "<strong>  Científica y Tecnológica.</strong><br> Interactúa compartiendo  información, ideas y opiniones sobre aspectos científicos y tecnológicos de su entorno inmediato en distintas situaciones de comunicación.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },
                    { "<strong>  Resolución de Problemas.</strong><br>  Interactúa ofreciendo indicaciones e instrucciones de forma oral y escrita, expresando opiniones sobre problemas y situaciones de su entorno.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },
                    { "<strong>  Ambiental y de la Salud.</strong><br> Muestra cuidado de su salud y la de los demás y respeto por el medioambiente, al interactuar de forma breve y sencilla sobre actividades cotidianas y preferencias en distintas situaciones de comunicaciones", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },
                    { "<strong>  Desarrollo Personal y Espiritual.</strong><br>  Se comunica con cortesía, asertividad, actitud de respeto y honestidad, al expresarse sobre sí mismo y los demás, en cuanto a preferencias, experiencias, características personales y actividades cotidianas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "ING" } },

                    #endregion
                    
                    #region Francés

                    { "<strong> Comunicativa. </strong><br> Comprende y expresa ideas, sentimientos y valores culturales en distintas situaciones de comunicación, con la intención de saludar, despedirse, interactuar, presentarse y presentar a otros, referirse a actividades cotidianas, describir e indicar ubicación.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Interactúa con el propósito de describir e indicar ubicación en distintas situaciones de comunicación.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },
                    { "<strong> Ética y Ciudadana. </strong><br>  Se comunica de forma oral y escrita en intercambios sencillos con cortesía, asertividad y respeto, a fin de identificar diferencias individuales, identidad social y cultural propia y de países francófonos.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },
                    { "<strong>  Científica y Tecnológica.</strong><br> Se comunica de forma oral o escrita, con el propósito de indicar ubicación e interactuar en distintas situaciones de comunicación de manera presencial o virtual.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },
                    { "<strong>  Resolución de Problemas.</strong><br> Se comunica con propósito de interactuar e indicar ubicación de personas, lugares y objetos distintas situaciones de comunicación.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },
                    { "<strong>  Ambiental y de la Salud.</strong><br>  Interactúa con el propósito de expresar preferencias por actividades cotidianas y opciones que impactan positivamente la salud y el medioambiente.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },
                    { "<strong>  Desarrollo Personal y Espiritual.</strong><br>  Se comunica con cortesía, asertividad, actitud de respeto, honestidad y aceptación, con el propósito de presentarse y describirse, a sí mismo y a otras personas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FRA" } },

                    #endregion

                    #region Matemática

                    { "<strong> Comunicativa. </strong><br> Interpreta ideas haciendo uso de la numeración matemática dentro de los sistemas numéricos, con el fin de relacionarlos con los conceptos de activos, pasivos, patrimonio, ingresos, gastos, y costos para organizar las finanzas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "MAT" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Explica lógicamente situaciones de la comunidad sobre conocimientos básicos de la geometría.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "MAT" } },
                    { "<strong>  Científica y Tecnológica.</strong><br>  Analiza diferentes tipos de herramientas tecnológicas para la resolución de problemas en la que interviene la numeración y la estadística.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "MAT" } },
                    { "<strong> Ética y Ciudadana. </strong><br> Interpreta situaciones que impliquen conocimientos de medición respetando diferentes puntos de vistas y asumiendo actitud responsable.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "MAT" } },
                    { "<strong> Ambiental y de la Salud. </strong><br>  Usa los conocimientos de numeración y estadística, con el fin de contribuir a la solución de situaciones de la comunidad para una vida saludable.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "MAT" } },
                    { "<strong>  Desarrollo Personal y Espiritual.</strong><br> Asume una actitud responsable en la interpretación de situaciones que impliquen  conocimientos de medición respetando diferentes puntos de vistas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "MAT" } },

                    #endregion

                    #region Ciencias Sociales 

                    { "<strong> Comunicativa. </strong><br> Indaga en fuentes primarias y secundarias sobre la geografía y sus ciencias auxiliares, con la finalidad de obtener informaciones confiables.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Relaciona en propuestas y proyectos de investigación, hechos históricos con el espacio geográfico en diferentes periodos, con la finalidad de desarrollar una conciencia crítica.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },
                    { "<strong>  Ética y Ciudadana.</strong><br> Realiza propuestas que fomenten la interacción sociocultural, el respeto a la Constitución y la construcción ciudadana, con la finalidad de afianzar la democracia, la cultura de paz y el respeto a los derechos humanos.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },
                    { "<strong>   Científica y Tecnológica. </strong><br> Utiliza en investigaciones realizadas, planteamientos y teorías sobre el surgimiento del nacionalismo y la incidencia del Antillanismo en el Caribe, con la finalidad de realizar explicaciones científicas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },
                    { "<strong>  Resolución de Problemas.  </strong><br> Verifica en el levantamiento de informaciones, la existencia de un problema; con la finalidad de ubicarlo en el contexto social en el que se producen.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },
                    { "<strong> Ambiental y de la Salud. </strong><br>  Cuestiona acciones humanas que pueden generar daños a estilos de vida saludables y al equilibrio ambiental, con la finalidad de conocer sus efectos en la sociedad y la naturaleza.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },
                    { "<strong>  Desarrollo Personal y Espiritual.</strong><br>  Emplea relaciones armoniosas y equilibradas, con la finalidad de promover la construcción de una cultura de paz, basada en el respeto a su persona y a los demás.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIESOC" } },

                    #endregion

                    #region Ciencias de la Naturaleza

                    { "<strong> Comunicativa. </strong><br> Se comunica utilizando el lenguaje científico y tecnológico de ciencias de la tierra y el universo.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Ofrece explicaciones científicas y tecnológicas a problemas y fenómenos naturales relacionados a Ciencias de la Tierra y el Universo.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },
                    { "<strong>  Resolución de Problemas.  </strong><br> Aplica procedimientos científicos y tecnológicos para solucionar problemas o dar respuestas a fenómenos naturales relacionados con Ciencias de la Tierra y el Universo.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },
                    { "<strong>   Científica y Tecnológica. </strong><br>  Se cuestiona e identifica problemas y situaciones utilizando ideas y procesos fundamentales de las Ciencias de la Tierra y el Universo.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },
                    { "<strong>  Ética y Ciudadana.</strong><br> Analiza la naturaleza de las Ciencias Naturales y el desarrollo tecnológico relacionado con Ciencias de la Tierra y el Universo", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },
                    { "<strong> Ambiental y de la Salud. </strong><br>  Actúa con responsabilidad crítica y autónoma para el cuidado de su salud y ambientales relacionadas con Ciencias de la Tierra y el Universo.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },
                    { "<strong>  Desarrollo Personal y Espiritual.</strong><br> Gestiona actitudes intelectuales, emocionales y conductuales proactivas al desarrollo de su proyecto personal desde las Ciencias de la Tierra y el Universo.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "CIEDELANAT" } },

                    #endregion

                    #region Educación Artística

                    { "<strong> Comunicativa. </strong><br>  Realiza procesos creativos utilizando técnicas y estilos artísticos en relación con la intención comunicativa, expresando síntesis de ideas, sentimientos y vivencias, tanto propias como presentes en obras diversas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Crea obras artísticas a partir de los aportes de los diversos diálogos culturales, valorando las expresiones artísticas del sincretismo cultural e identificando sus elementos constitutivos.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },
                    { "<strong> Resolución de Problemas.  </strong><br>  Argumenta sobre la función de distintos lenguajes artísticos en la construcción histórica de la sociedad en los contextos diversos.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },
                    { "<strong> Científica y Tecnológica. </strong><br>   Identifica recursos tecnológicos de diferentes lenguajes artísticos en la realización o análisis de producciones artísticas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },
                    { "<strong> Ética y Ciudadana.</strong><br> Comunica ideas, emociones, sentimientos y vivencias, a través de géneros, técnicas y estilos de diversas manifestaciones artísticas, valorando su capacidad comunicativa con el contexto.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },
                    { "<strong> Ambiental y de la Salud. </strong><br>  Reconoce las posibilidades técnicas que ofrecen las artes para la valoración y uso consciente de los recursos y espacios naturales.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },
                    { "<strong> Desarrollo Personal y Espiritual.</strong><br> Reconoce las manifestaciones artísticas multidisciplinares, identificando sus componentes e influencias, y reconociendo su trascendencia.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUART" } },

                    #endregion

                    #region Educación Física

                    { "<strong> Comunicativa. </strong><br>  Utiliza su cuerpo para expresarse en relación armónica con las demás personas y su entorno social y cultural.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },
                    { "<strong> Ética y Ciudadana.</strong><br> Realiza acciones motrices diversas en relación con los demás, mostrando respeto y responsabilidad.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },
                    { "<strong> Ambiental y de la Salud. </strong><br> Reconoce algunos cambios en su cuerpo y sus habilidades motoras, con el fin de valorarlos evitando situaciones de riesgo para la salud.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },
                    { "<strong> Desarrollo Personal y Espiritual.</strong><br>Identifica sus habilidades motrices y capacidades físicas en el desarrollo de actividades corporales diversas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },
                    { "<strong> Científica y Tecnológica. </strong><br>  Identifica las condiciones del perímetro de juegos y deportes.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Muestra nivel de dominio en su acción motriz a partir de la noción de su esquema corporal, relaciones espaciales y temporales estáticas.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },
                    { "<strong> Resolución de Problemas.  </strong><br> Muestra niveles básicos de desempeño motriz en situaciones de juego, a partir de sus condiciones físicas naturales.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "EDUFÍS" } },

                    #endregion

                    #region  Formación Integral Humana y Religiosa

                    { "<strong> Ética y Ciudadana.</strong><br> Descubre en los evangelios la novedad de la vida y los valores practicados por Jesús de Nazaret, a fin de compararlos con los asumidos en su familia, escuela y comunidad e integrarlos en su relación con los demás.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong> Resolución de Problemas.  </strong><br>  Constata las principales dificultades que viven los adolescentes en su desarrollo y en sus relaciones sociales y familiares a fin de buscar alternativas de solución a los conflictos que se les presenten.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong> Ambiental y de la Salud. </strong><br>  Identifica acciones a favor del cuidado de la vida, en todas sus manifestaciones, a fin de garantizar su salud personal y colectiva, desde los diferentes contextos.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong> Comunicativa. </strong><br>  Constata el respeto a la vida presente en las manifestaciones culturales y religiosas de su familia y comunidad, a fin de promover y asimilar derechos humanos, valores morales y religiosos, en los diferentes contextos.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong> Pensamiento Lógico, Creativo y Crítico. </strong><br> Presenta la importancia del trabajo, a fin de promover su implementación para la calidad de vida de las personas, la familia y la comunidad.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FORINTHUMYREL" } },
                    { "<strong> Científica y Tecnológica. </strong><br> Constata la importancia de aplicar criterios éticos en el uso de las ciencias y las tecnologías, a fin de mejorar la calidad de vida de las personas y del entorno natural.", new AchievementIndicatorInfo { Grade = "Primero Secundaria", CodeSubject = "FORINTHUMYREL" } },

                    #endregion

                 #endregion                         

            };


            foreach (var grade in competencias)
            {
                var viewModel = new SaveAchievementIndicatorsViewModel
                {
                    Id = idCounter,
                    Description = grade.Key,
                    AssociatedGrades = grade.Value.Grade,
                    CodeSubject = grade.Value.CodeSubject,
                    CreatedBy = "Admin",
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LastModifiedBy = "Admin"
                };
                await indicatorsService.Add(viewModel);
                idCounter++;
            }

        }

        public static async Task SetMultipleSubjects(ISujectService _sujectService)
        {
            var subjects = new List<SujectSaveViewModel>
            {
                new SujectSaveViewModel
                {
                    Name = "Lengua Española",
                    Code = "LENESP",
                    Description = "Desarrollo de la comunicación oral y escrita.",
                },
                new SujectSaveViewModel
                {
                    Name = "Matemáticas",
                    Code = "MAT",
                    Description = "Pensamiento lógico, resolución de problemas y razonamiento numérico.",
                },
                new SujectSaveViewModel
                {
                    Name = "Ciencias Sociales",
                    Code = "CIESOC",
                    Description = "Historia, cultura y entorno social.",
                },
                new SujectSaveViewModel
                {
                    Name = "Ciencias de la Naturaleza",
                    Code = "CIEDELANAT",
                    Description = "Exploración y comprensión de fenómenos naturales.",
                },
                new SujectSaveViewModel
                {
                    Name = "Educación Física",
                    Code = "EDUFÍS",
                    Description = "Desarrollo corporal, motricidad y salud física.",
                },
                new SujectSaveViewModel
                {
                    Name = "Formación Integral, Humana y Religiosa",
                    Code = "FORINTHUMYREL",
                    Description = "Valores, espiritualidad y convivencia.",
                },
                new SujectSaveViewModel
                {
                    Name = "Educación Artística",
                    Code = "EDUART",
                    Description = "Expresión artística, creatividad e identidad cultural.",
                },
                new SujectSaveViewModel
                {
                    Name = "Lenguas Extranjeras (Inglés)",
                    Code = "ING",
                    Description = "Comprensión y expresión oral y escrita básica en inglés, aplicada a situaciones cotidianas.",
                },
                new SujectSaveViewModel
                {
                    Name = "Lenguas Extranjeras (Francés)",
                    Code = "FRA",
                    Description = "Expresión de ideas, sentimientos y ubicación en francés, con respeto y valores culturales.",
                }
            };

            foreach (var subject in subjects)
            {
                await _sujectService.Add(subject);
            }
        }


    }
}
