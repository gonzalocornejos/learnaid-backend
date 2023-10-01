namespace learnaid_backend.Core.Integrations.OpenAI
{
    using Interfaces;
    using learnaid_backend.Configuration;
    using learnaid_backend.Core.DataTransferObjects.Ejercicio;
    using Microsoft.Extensions.Options;
    using OpenAI;
    using OpenAI_API.Models;
    using System.Threading.Tasks;

    public class OpenAIAPI : IOpenAIAPI
    {
        private readonly OpenAIConfiguration _openAIConfiguration;

        public OpenAIAPI(IOptionsMonitor<OpenAIConfiguration> optionsMonitor)
        {
            _openAIConfiguration = optionsMonitor.CurrentValue;
        }

        /*
        public async Task<string> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio, string profesion)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + ejercicio.EdadAlumnos + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que simplificar el texto del ejercicio, si se puede acortarlo mejor, pero nunca alargarlo ni cambiar su contenido, separar los ejercicios en items individuales y poner opciones multiples para las respuestas. Las opciones multiples tienen que ser todas posibles respuestas validas y la opcion correcta no tiene que estar siempre en el mismo lugar.Que haya solo un espacio para completar por ejercicio. Que todas las palabras clave del ejercicio esten en mayusculas para resaltar su importancia.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("El ejercicio esta en ingles. La estructura de tu respuesta tiene que ser: consigna-ejercicio-opciones. El ejercicio es el siguiente: CONSIGNA:FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: EJERCICIO:What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("CONSIGNA: Completa los espacios en blanco con los verbos en pasado simple. \n EJERCICIO: \n What __________ you _____________(do) last Saturday? \n OPCIONES: \n \t \t \t a) did, do \n \t \t \t b) did, did \n \t \t \t c) do, did \n \t \t \t d) did, did not \n EJERCICIO: \n I ______________________(visit) my grandparents. \n OPCIONES: \n \t \t \t a) visited \n \t \t \t b) visit \n \t \t \t c) visits \n \t \t \t d) visiting \n EJERCICIO: \n I ____________________(not go) alone. \n OPCIONES: \n \t \t \t a) did not go \n \t \t \t b) don't go \n \t \t \t c) didn't go \n \t \t \t d) not go \n EJERCICIO: \n My brother _________________(go) with me. \n OPCIONES: \n \t \t \t a) went \n \t \t \t b) go \n \t \t \t c) goes \n \t \t \t d) going");
            chat.AppendUserInput("El ejercicio esta en español.La estructura de tu respuesta tiene que ser: consigna - ejercicio - opciones.CONSIGNA: Resuelva las siguientes ecuaciones: EJERCICIO: 2 - x = x - 8");
            chat.AppendExampleChatbotOutput("CONSIGNA: Resuelve la siguiente ecuación: \n EJERCICIO: \n 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'
            var estructura = "";
            if(ejercicio.Texto == "")
            {
                estructura = "consigna-ejercicio-opciones";
            } else
            {
                estructura = "consigna-texto-ejercicio-opciones";
            }

            var pregunta = "El ejercicio esta en" + ejercicio.Idioma + ". Tu respuesta tiene que estar escrita en ese idioma. La estructura de tu respuesta tiene que ser:" + estructura + ". El ejercicio es el siguiente: CONSIGNA:" + ejercicio.Consigna + " EJERCICIO:" + ejercicio.Ejercicio;
            chat.AppendUserInput(pregunta);
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);
            return (response);
        }*/

        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {

            var respuesta = new EjercicioAdaptadoDTO();
            var adaptacionParcial = ejercicio;
            foreach(var adaptacion in ejercicio.Adaptaciones)
            {
                 if (adaptacion == "Seleccion multiple")
                {
                    respuesta = await AgregarMultipleChoice(adaptacionParcial,profesion,edad, idioma);
                    adaptacionParcial.Consigna = respuesta.Consigna;
                    adaptacionParcial.Ejercicio = respuesta.Ejercicio;
                } else if (adaptacion == "Acortar ejercicio")
                {
                    respuesta = await AcortarEjercicio(adaptacionParcial, profesion, edad, idioma);
                    adaptacionParcial.Consigna = respuesta.Consigna;
                    adaptacionParcial.Ejercicio = respuesta.Ejercicio;
                } else if (adaptacion == "Simplificar ejercicio")
                {
                    respuesta = await SimplificarTexto(adaptacionParcial, profesion, edad, idioma);
                    adaptacionParcial.Consigna = respuesta.Consigna;
                    adaptacionParcial.Ejercicio = respuesta.Ejercicio;
                }
            }
            respuesta = await AdaptacionGeneral(adaptacionParcial, profesion, edad, idioma);
            return respuesta;
        }

        public async Task<EjercicioAdaptadoDTO> AdaptacionGeneral(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();
            chat.Model = Model.ChatGPTTurbo;

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. Lo que tenes que hacer es leer todo: la consigna, el texto y el ejercicio. Luego tenes que identificar todas las palabras importantes que permitan resolver el ejercicio con mayor facilidad. Aquella que sean importante las escribis en mayusculas. Tambien tenes que separar todos los ejercicios en items individuales. Por ejemplo, a un parrafo donde el alumno tenga que completar dentro de el, separarlo por oraciones individuales. Y numerar todo. No agregues palabras extra como Consigna,Ejercicio o Texto. No modifiques nunca el contenido de las oraciones, no agregues adjetivos o palabras, ni tampoco cambies el contenido de las preguntas.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL in the blanks with the verbs in the PAST SIMPLE:");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) LAST Saturday? \n 2)I ______________________(visit) my grandparents. \n 3) I ____________________(not go) alone. \n 4) My brother _________________(go) with me. ");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Resuelva las siguientes ecuaciones:. El ejercicio es el siguiente: 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");
            chat.AppendExampleChatbotOutput("1) 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);

        }

        public async Task<EjercicioAdaptadoDTO> AcortarEjercicio(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();
            chat.Model = Model.ChatGPTTurbo;
            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar el formato de ejercicios para ellos. Lo que tenes que hacer es leer todo: la consigna, el texto y el ejercicio. Luego tenes que identificar cuales son los ejercicio que tiene que resolver el alumno. Una vez hecho eso, tenes que eliminar la mitad de los ejercicios y dejar solo la otra mitad.Esto puede consistir en eliminar oraciones, o eliminar preguntas, o eliminar ecuaciones, o eliminar parte de un parrafo.Todo depende el contexto.No agregues palabras extra como Consigna, Ejercicio o Texto. No modifiques nunca el contenido de las oraciones, no agregues adjetivos o palabras, ni tampoco cambies el contenido de las preguntas.";
            chat.AppendSystemMessage(system);  

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) last Saturday? \n \n \t \t \t a) did, do \n \t \t \t b) did, did \n \t \t \t c) do, did \n \t \t \t d) did, did not \n \n 2)I ______________________(visit) my grandparents. \n \n \t \t \t a) visited \n \t \t \t b) visit \n \t \t \t c) visits \n \t \t \t d) visiting \n \n 3) I ____________________(not go) alone. \n \n \t \t \t a) did not go \n \t \t \t b) don't go \n \t \t \t c) didn't go \n \t \t \t d) not go \n \n 4) My brother _________________(go) with me. \n \n \t \t \t a) went \n \t \t \t b) go \n \t \t \t c) goes \n \t \t \t d) going");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Responda las siguientes preguntas:. El ejercicio es el siguiente: ¿Qué órgano de tu cuerpo se encarga de bombear la sangre?¿Cuál es la función principal de los pulmones?¿Qué insecto pasa por una etapa de larva antes de convertirse en adulto?¿Qué parte de la planta absorbe agua y nutrientes del suelo?¿Cómo se llama el proceso en el cual las plantas fabrican su propio alimento utilizando la energía del sol?¿Qué animal es conocido por cambiar de color para camuflarse con su entorno?¿Qué animal es famoso por construir grandes presas de palos y ramas en los ríos?¿Cuál es el órgano más grande de tu cuerpo?¿Qué animal vuela de noche y es conocido por girar la cabeza casi 180 grados?¿Cuál es la función de la piel en nuestro cuerpo?");
            chat.AppendExampleChatbotOutput("1) ¿Qué órgano bombea sangre?\n2) ¿Cuál es la función principal de los pulmones?\n3) ¿Qué insecto pasa por etapa de larva antes de ser adulto?\n4) ¿Qué parte de la planta absorbe agua y nutrientes?\n5) ¿Cómo se llama el proceso en el cual las plantas fabrican su propio alimento?");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);
        }

        public async Task<EjercicioAdaptadoDTO> AgregarMultipleChoice(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            chat.Model = Model.ChatGPTTurbo;
            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. Lo que tenes que hacer es leer todo: el ejercicio, la consigna y el texto. Luego tenes que identificar las respuestas correctas para cada ejercicio. Luego tenes que agregar opciones multiples debajo de cada ejercicio. Dentro de las opciones tiene que estar la correcta y rellenar las otras con posibles opciones que serian correctas bajo otro contexto. Que la opcion correcta no este siempre en el mismo lugar. No agregues palabras extra como Consigna, Ejercicio o Texto. No modifiques nunca el contenido de las oraciones, no agregues adjetivos o palabras, ni tampoco cambies el contenido de las preguntas.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) last Saturday? \n \n \t \t \t a) did, do \n \t \t \t b) did, did \n \t \t \t c) do, did \n \t \t \t d) did, did not \n \n 2)I ______________________(visit) my grandparents. \n \n \t \t \t a) visited \n \t \t \t b) visit \n \t \t \t c) visits \n \t \t \t d) visiting \n \n 3) I ____________________(not go) alone. \n \n \t \t \t a) did not go \n \t \t \t b) don't go \n \t \t \t c) didn't go \n \t \t \t d) not go \n \n 4) My brother _________________(go) with me. \n \n \t \t \t a) went \n \t \t \t b) go \n \t \t \t c) goes \n \t \t \t d) going");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Resuelva las siguientes ecuaciones:. El ejercicio es el siguiente: 2 - x = x - 8 ");
            chat.AppendExampleChatbotOutput("1) 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);
        }

        public async Task<EjercicioAdaptadoDTO> SimplificarTexto (EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();
            chat.Model = Model.ChatGPTTurbo;

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. Tene en cuenta que leer mucho texto hace que sea mas dificil. Para adaptar el ejercicio tenes que leer todo: la consigna, el texto y el ejercicio. De ahi identificar todo el texto necesario para responder al ejercicio. Luego, tenes que acortar el texto para que solo quede el necesario para responder las preguntas. No agregas nada, solo acortas. No agregues palabras extra como Consigna,Ejercicio o Texto. No modifiques nunca el contenido de las oraciones, no agregues adjetivos o palabras, ni tampoco cambies el contenido de las preguntas.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: Lea el texto y responda las preguntas:");
            chat.AppendExampleChatbotOutput("Lea el texto y responda las preguntas:");
            chat.AppendUserInput("Adapta el siguiente texto en base a la consigna: Lea el texto y responda las preguntas:. El ejercicio es: ¿Dónde vivía Pedro? ¿Qué encontró Pedro mientras caminaba cerca del río ? ¿Qué conexión tenía Pedro con la naturaleza ? ¿Qué hizo Pedro después de encontrar el medallón?. El texto es el siguiente: En un tranquilo pueblo rodeado de montañas y bosques, vivía un joven llamado Pedro. Pedro siempre había sentido una fuerte conexión con la naturaleza y pasaba horas explorando los senderos del bosque. Un día, mientras caminaba cerca de un río, encontró un extraño objeto brillante en el suelo. Era un medallón con inscripciones antiguas. Intrigado, Pedro decidió investigar su significado y descubrió que el medallón estaba relacionado con una antigua leyenda sobre un tesoro escondido en las profundidades del bosque. A partir de ese momento, Pedro se embarcó en una emocionante búsqueda llena de desafíos y aventuras.");
            chat.AppendExampleChatbotOutput("Pedro vivía en un pueblo rodeado de montañas. Un día, encontró un medallón brillante cerca de un río. Le gustaba explorar la naturaleza. Decidió investigar el medallón y descubrió una leyenda sobre un tesoro. Comenzó una búsqueda emocionante.");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Lea el texto y responda las preguntas:. El ejercicio es el siguiente: ¿Dónde vivía Pedro? ¿Qué encontró Pedro mientras caminaba cerca del río ? ¿Qué conexión tenía Pedro con la naturaleza ? ¿Qué hizo Pedro después de encontrar el medallón?");
            chat.AppendExampleChatbotOutput("1)¿Dónde vivía Pedro? \n 2)¿Qué encontró Pedro mientras caminaba cerca del río ? \n 3)¿Qué conexión tenía Pedro con la naturaleza ? \n 4)¿Qué hizo Pedro después de encontrar el medallón?");
            chat.AppendUserInput("Adapta el siguiente texto en base a la consigna: Lea el texto y responda las preguntas:. El ejercicio es: ¿Qué brilla en el cielo?¿Qué hacen las aves entre los árboles?¿Dónde observa el gato a las mariposas?¿Qué hacen los niños en el jardín?¿Qué se siente en el aire?. El texto es el siguiente: El sol brilla cálidamente en el amplio cielo azul, y las aves vuelan con absoluta libertad entre las frondosas ramas de los antiguos árboles que se alzan majestuosamente. En un tranquilo rincón sombreado del exuberante jardín, un gato curioso de pelaje suave y ojos brillantes observa con gran atención a las coloridas mariposas que revolotean juguetonamente cerca de las delicadas flores que despliegan su belleza. Los niños, llenos de energía y entusiasmo, se entregan a sus juegos en el suave y verde césped, mientras que el suave y reconfortante aroma de las diversas flores que despliegan una paleta de colores, perfuma dulcemente el fresco aire que acaricia la piel. Verdaderamente, es un día absolutamente perfecto para entregarse sin reservas a disfrutar plenamente al aire libre y así sumergirse en la inigualable alegría que emana de la profunda conexión con la madre naturaleza,esa que siempre nos brinda su regalo eterno de serenidad y vitalidad.");
            chat.AppendExampleChatbotOutput("El sol brilla en el cielo. Las aves vuelan entre los árboles. El gato observa a las mariposas en un rincón sombreado del jardín. Los niños juegan en el césped. Se siente el aroma de las flores en el aire.");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Lea el texto y responda las preguntas:. El ejercicio es el siguiente: ¿Qué brilla en el cielo?¿Qué hacen las aves entre los árboles?¿Dónde observa el gato a las mariposas?¿Qué hacen los niños en el jardín?¿Qué se siente en el aire?");
            chat.AppendExampleChatbotOutput("1)¿Qué brilla en el cielo?\n 2)¿Qué hacen las aves entre los árboles?\n 3)¿Dónde observa el gato a las mariposas?\n 4)¿Qué hacen los niños en el jardín?\n 5)¿Qué se siente en el aire?");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);
        }

        public async Task<EjercicioAdaptadoDTO> GenerarRespuesta(EjercicioPorAdaptarDTO ejercicio, OpenAI_API.Chat.Conversation chat, string idioma)
        {
            var respuesta = new EjercicioAdaptadoDTO();
            var pregunta = "El idioma es " + idioma + ".Adapta la siguiente consigna: " + ejercicio.Consigna;
            chat.AppendUserInput(pregunta);
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);
            respuesta.Consigna = response;

            if (ejercicio.Texto != "")
            {
                pregunta = "El idioma es "+ idioma +".Adapta el siguiente texto en base a la consigna: " + ejercicio.Consigna + ". No incluyas la consigna en la respuesta. Y en base al siguiente ejercicio: "+ ejercicio.Ejercicio + ". No incluyas el ejercicio en la respuesta. El texto es el siguiente: " + ejercicio.Texto;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = response;
                pregunta = "El idioma es " + idioma + ".Adapta el siguiente ejercicio en base a la consigna y al texto siguiente: " + ejercicio.Consigna + ejercicio.Texto + ". El ejercicio es el siguiente: " + ejercicio.Ejercicio;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = respuesta.Ejercicio + "\n" + response;
            }
            else
            {
                pregunta = "El idioma es " + idioma + ".Adapta el siguiente ejercicio en base a la consigna: " + ejercicio.Consigna + ". El ejercicio es el siguiente: " + ejercicio.Ejercicio;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = response;
            }
            return respuesta;
        }

    }
}
