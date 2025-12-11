async function llamarApi() {
  let resultados = fetch("http://localhost:8941/api/Autor")
    .then((res) => res.json())
    .then((Response) => {
      console.log(Response);
      let table = document.getElementById("mitabla");

      Response.forEach((element) => {
        let linea = document.createElement("tr");
        let idTD = document.createElement("td");
        idTD.innerText = element.id;
        linea.appendChild(idTD);
        let nombreTD = document.createElement("td");
        nombreTD.innerText = element.nombre;
        linea.appendChild(nombreTD);
        let apellidoTD = document.createElement("td");
        apellidoTD.innerText = element.apellido;
        linea.appendChild(apellidoTD);
        table.appendChild(linea);
      });
    });
  let resultadoNormales = await JSON.stringify(resultados);
  console.log(resultadoNormales);
}
llamarApi();
