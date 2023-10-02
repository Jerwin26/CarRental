<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/5.0.0/dist/js/bootstrap.bundle.min.js"></script>
<script>
    (function () {
        $(document).ready(function () {
            // Validation functions
            function containsNumbers(input) {
                return /\d/.test(input);
            }

            function isValidEmail(email) {
                return /^[^\s]+@[^\s]+\.[^\s]+$/.test(email);
            }

            function isValidPhoneNumber(phone) {
                return /^\d{10}$/.test(phone); // Assuming 10-digit phone number
            }

            // Error handling functions
            function showError(field, message) {
                field.addClass("is-invalid");
                field.closest(".form-group").find(".text-danger").text(message);
            }

            function clearError(field) {
                field.removeClass("is-invalid");
                field.closest(".form-group").find(".text-danger").text("");
            }

            // Event handlers
            $("#firstName, #lastName").on("input", function () {
                var value = $(this).val();
                if (containsNumbers(value)) {
                    showError($(this), "Name should not contain numbers.");
                } else {
                    clearError($(this));
                }
            });

            $("#emailAddress").on("input", function () {
                var value = $(this).val();
                if (!isValidEmail(value)) {
                    showError($(this), "Invalid email address.");
                } else {
                    clearError($(this));
                }
            });

            $("#phoneNumber").on("input", function () {
                var value = $(this).val();
                if (!isValidPhoneNumber(value)) {
                    showError($(this), "Invalid phone number (10 digits required).");
                } else {
                    clearError($(this));
                }
            });

            $("#password, #ConfirmPassword").on("input", function () {
                var password = $("#password").val();
                var confirmPassword = $("#ConfirmPassword").val();
                if (password !== confirmPassword) {
                    showError($("#ConfirmPassword"), "Passwords do not match.");
                } else {
                    clearError($("#ConfirmPassword"));
                }
            });

            $("input[name='gender']").on("change", function () {
                var selectedGender = $("input[name='gender']:checked").val();
                if (!selectedGender) {
                    showError($(this).closest(".form-group"), "Please select a gender.");
                } else {
                    clearError($(this).closest(".form-group"));
                }
            });

            $("form").submit(function () {
                var isValid = true;

                $(this).find(":input[required]").each(function () {
                    if (!$(this).val()) {
                        showError($(this), "This field is required.");
                        isValid = false;
                    }
                });

                return isValid;
            });

            $("#stateDropdown").change(function () {
                var selectedStateId = $(this).val();
                if (selectedStateId !== "") {
                    $.ajax({
                        url: '@Url.Action("Cities", "Admin")',
                        type: 'GET',
                        data: { stateId: selectedStateId },
                        success: function (data) {
                            var cityDropdown = $("#cityDropdown");
                            cityDropdown.empty();
                            cityDropdown.append($('<option>').text("Select a City").val(""));
                            $.each(data, function (index, city) {
                                cityDropdown.append($('<option>').text(city.Text).val(city.Value));
                            });
                        }
                    });
                } else {
                    var cityDropdown = $("#cityDropdown");
                    cityDropdown.empty();
                    cityDropdown.append($('<option>').text("Select a City").val(""));
                }
            });
        });
    })();
</script>
